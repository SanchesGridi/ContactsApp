using System;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Autofac;

using ContactsApp.UI.IoC.Infra;
using ContactsApp.UI.ViewModels;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.Domain.Data.Containers;
using ContactsApp.Infrastructure.Data.Sql;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.Infrastructure.Data.Sql.Storage;

#nullable enable

namespace ContactsApp.UI.IoC.Locators
{
    public sealed class ApplicationLocatorWrapper
    {
        private readonly AutofacLocator _locator;

        public EnterPageViewModel EnterPageVm
        {
            get => _locator.GetInstance<EnterPageViewModel>();
        }
        public LoginPageViewModel LoginPageVm
        {
            get => _locator.GetInstance<LoginPageViewModel>();
        }
        public RegistrationPageViewModel RegistrationPageVm
        {
            get => _locator.GetInstance<RegistrationPageViewModel>();
        }
        public ContactsPageViewModel ContactsPageVm
        {
            get => _locator.GetInstance<ContactsPageViewModel>();
        }
        public ContactDefaultPageViewModel ContactDefaultPageVm
        {
            get => _locator.GetInstance<ContactDefaultPageViewModel>();
        }
        public ContactDataPageViewModel ContactDataPageVm
        {
            get => _locator.GetInstance<ContactDataPageViewModel>();
        }
        public TelegramAccountPageViewModel TelegramAccountPageVm
        {
            get => _locator.GetInstance<TelegramAccountPageViewModel>();
        }
        public NoteWindowViewModel NoteWindowVm
        {
            get => _locator.GetInstance<NoteWindowViewModel>();
        }
        public AdminPageViewModel AdminPageVm
        {
            get => _locator.GetInstance<AdminPageViewModel>();
        }
        public ISqlContainer SqlContainer
        {
            get => _locator.GetInstance<ISqlContainer>();
        }

        public ApplicationLocatorWrapper()
        {
            var linker = new ApplicationModuleLinker();
            var module = new MacroModule(linker.GetModules());

            _locator = new AutofacLocator(module);
        }

        private sealed class ApplicationModuleLinker
        {
            public IEnumerable<IModule> GetModules()
            {
                return new IModule[]
                {
                    this.CreateServices(),
                    this.CreateViewModels(),
                };
            }

            private Func<ParameterInfo, IComponentContext, bool> GetPredicate(Type type, string name)
            {
                return new Func<ParameterInfo, IComponentContext, bool>((pi, ctx) => pi.ParameterType == type && pi.Name == name);
            }
            private Func<ParameterInfo, IComponentContext, object?> GetValueAccessor<TAny>() where TAny : notnull
            {
                return new Func<ParameterInfo, IComponentContext, object?>((pi, ctx) => ctx.Resolve<TAny>());
            }

            private IModule CreateServices()
            {
                var resource = this.CreateServiceComponent(typeof(ApplicationResourceService), typeof(IResourceService), RegistrationFlag.Singleton);
                var searcher = this.CreateServiceComponent(typeof(ObjectSearcher), typeof(IObjectSearcher), RegistrationFlag.Singleton);
                var dialog = this.CreateServiceComponent(typeof(DialogService), typeof(IDialogService), RegistrationFlag.Singleton);
                var messanger = this.CreateServiceComponent(typeof(MessageBoxService), typeof(IMessageBoxService), RegistrationFlag.Singleton);
                var cmdProvider = this.CreateServiceComponent(typeof(DefaultCommandProvider), typeof(ICommandProvider), RegistrationFlag.PerDependency);
                var navigation = this.CreateServiceComponent(typeof(UiNavigation), typeof(IUiNavigation), RegistrationFlag.PerDependency);
                var windowStarter = this.CreateServiceComponent(typeof(WindowStarter), typeof(IWindowStarter), RegistrationFlag.PerDependency);

                const string localDb = @"(LocalDB)\MSSQLLocalDB";
                const bool initialize = false; // :-)
                var context = this.CreateServiceComponent(typeof(ApplicationContext), typeof(DbContext), RegistrationFlag.PerDependency);
                context.SetCtorArgs(new Autofac.Core.Parameter[]
                {
                    new NamedParameter("info", new MsSqlStorageInfo("A_Contacts_G", localDb)),
                    new NamedParameter("initialize", initialize)
                });
                
                var container = this.CreateServiceComponent(typeof(SqlContainer), typeof(ISqlContainer), RegistrationFlag.PerDependency);
                container.SetCtorArgs(new Autofac.Core.Parameter[] 
                { 
                    new Autofac.Core.ResolvedParameter(this.GetPredicate(typeof(DbContext), "context"), this.GetValueAccessor<DbContext>()),
                    new NamedParameter("repositoryRealizationType", typeof(SqlRepository<>))
                });

                var enumerable = new IManagedComponent[]
                {
                    resource,
                    searcher,
                    dialog,
                    messanger,
                    cmdProvider,
                    navigation,
                    windowStarter,

                    context,
                    container
                };

                return new StdModule(enumerable);
            }
            private IModule CreateViewModels()
            {
                var enterVm = this.CreateViewModel(typeof(EnterPageViewModel));
                var loginVm = this.CreateViewModel(typeof(LoginPageViewModel));
                var registrationVm = this.CreateViewModel(typeof(RegistrationPageViewModel));
                var contactsVm = this.CreateViewModel(typeof(ContactsPageViewModel));
                var contactDefaultVm = this.CreateViewModel(typeof(ContactDefaultPageViewModel));
                var contactDataVm = this.CreateViewModel(typeof(ContactDataPageViewModel));
                var telegramVm = this.CreateViewModel(typeof(TelegramAccountPageViewModel));
                var noteVm = this.CreateViewModel(typeof(NoteWindowViewModel));
                var adminVm = this.CreateViewModel(typeof(AdminPageViewModel));

                var enumerable = new IManagedComponent[]
                {
                    enterVm,
                    loginVm,
                    registrationVm,
                    contactsVm,
                    contactDefaultVm,
                    contactDataVm,
                    telegramVm,
                    noteVm,
                    adminVm,
                };

                return new StdModule(enumerable);
            }

            private IManagedComponent CreateServiceComponent(Type @class, Type @interface, RegistrationFlag regFlag)
            {
                var target = this.CreateTarget(@class, @interface);
                var info = this.CreateInfo(regFlag);

                return this.CreateComponent(target, info);
            }
            private IManagedComponent CreateViewModel(Type @class)
            {
                var target = this.CreateTarget(@class, null);
                var info = this.CreateInfo(RegistrationFlag.SelfRegistration);

                return this.CreateComponent(target, info, false);
            }

            private KeyValuePair<Type, Type> CreateTarget(Type classType, Type? interfaceType)
            {
                return new KeyValuePair<Type, Type>(classType, interfaceType);
            }
            private IBindingInformation CreateInfo(RegistrationFlag rFlag)
            {
                return new StdBindingInformation(rFlag);
            }
            private IManagedComponent CreateComponent(KeyValuePair<Type, Type> target, IBindingInformation info, bool includeInterface = true)
            {
                return new StdManagedComponent(target, info, includeInterface);
            }
        }
    }
}