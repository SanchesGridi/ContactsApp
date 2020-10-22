using System;
using System.Windows.Input;
using System.Collections.Generic;

using GalaSoft.MvvmLight;

using ContactsApp.UI.UiServices;
using ContactsApp.UI.Models.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.ViewModels.Base
{
    public abstract class BindingViewModel : ViewModelBase
    {
        private readonly Dictionary<string, CommandChangedEventHandler> _subscribersDictionary;

        private protected readonly ICommandProvider _cmdProvider;
        private protected readonly IUiNavigation _navigation;
        private protected readonly IMessageBoxService _messanger;
        private protected readonly IObjectSearcher _objectSearcher;
        private protected readonly IResourceService _resourceService;

        public BindingViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
        {
            _subscribersDictionary = new Dictionary<string, CommandChangedEventHandler>();

            _cmdProvider = cmdProvider;
            _navigation = navigation;
            _messanger = messanger;
            _objectSearcher = objectSearcher;
            _resourceService = resourceService;
        }

        private protected void Verify<TClass>(TClass instance) where TClass : class
        {
            instance.VerifyReference(nameof(instance));
        }
        private protected TClass VerifyAndSet<TClass>(TClass instance) where TClass : class
        {
            return instance.VerifyReferenceAndSet(nameof(instance));
        }
        private protected void UnsubscribeCanExecuteCommand(NotificationReflectorModel model, string propertyName)
        {
            var guaranteedCommandName = model.GetKeyCommand(propertyName);

            _cmdProvider.RemoveCommand(guaranteedCommandName);

            model.CanExecuteCommandChanged -= _subscribersDictionary[guaranteedCommandName];

            _subscribersDictionary.Remove(guaranteedCommandName);
        }
        private protected void SubscribeCanExecuteCommand(NotificationReflectorModel model, string propertyName, ICommand command)
        {
            bool raiseFunc(string cmdName) => _cmdProvider.RaiseRelayDefaultCommand(cmdName);

            this.SubscribeCanExecuteCommandInternal(model, propertyName, command, raiseFunc);
        }
        private protected void SubscribeCanExecuteCommand<TAny>(NotificationReflectorModel model, string propertyName, ICommand command)
        {
            bool raiseFunc(string cmdName) => _cmdProvider.RaiseRelayGenericCommand<TAny>(cmdName);

            this.SubscribeCanExecuteCommandInternal(model, propertyName, command, raiseFunc);
        }
 
        private void SubscribeCanExecuteCommandInternal(NotificationReflectorModel model, string propertyName, ICommand command, Func<string, bool> raiseFunc)
        {
            var guaranteedCommandName = model.GetKeyCommand(propertyName);

            _cmdProvider.AddCommand(guaranteedCommandName, command);

            CommandChangedEventHandler handler = (objectSender, commandName) =>
            {
                var result = raiseFunc.Invoke(commandName);

                if (!result)
                {
                    raiseFunc.Invoke(guaranteedCommandName);
                }
            };

            model.CanExecuteCommandChanged += handler;

            _subscribersDictionary.Add(guaranteedCommandName, handler);
        }
    }
}