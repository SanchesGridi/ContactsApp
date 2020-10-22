using System.IO;
using System.Threading;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.Views.Pages;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.IoC.Locators;
using ContactsApp.UI.Models.UiModels;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.Infrastructure.Data.Sql;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

// TODO: if navi back SourceCancel()
// TODO: password box binding investigate
// TODO: hidden functionality for registering users with admin rights

namespace ContactsApp.UI.ViewModels
{
    public class RegistrationPageViewModel : UiMarshalingViewModel
    {
        private enum RegistrationRole // ... TODO
        {
            User = 0,
            Admin = 1
        }

        private RegistrationRole _registrationRole; // ... TODO
        private SwitchModel _buttonSwitcher;
        private BinAccessModel _progressAccess;
        private UserModel _userModel;

        public SwitchModel ButtonSwitcher
        {
            get => _buttonSwitcher ?? (_buttonSwitcher = new SwitchModel(true));
            set => this.SetValue(ref _buttonSwitcher, value, nameof(ButtonSwitcher));
        }
        public BinAccessModel ProgressAccess
        {
            get => _progressAccess ?? (_progressAccess = new BinAccessModel(false));
            set => this.SetValue(ref _progressAccess, value, nameof(ProgressAccess));
        }
        public UserModel UserModel
        {
            get => _userModel ?? (_userModel = new UserModel());
            set => this.SetValue(ref _userModel, value, nameof(UserModel));
        }
        public ICommand CreateAccountCmd { get; }

        public RegistrationPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            CreateAccountCmd = new RelayCommand(this.CreateAccountExecute);
        }

        private async void CreateAccountExecute()
        {
            await base.MarshalActionAsync(() => ProgressAccess.ChangeAccess());
            await base.MarshalActionAsync(() => this.CreateAccountInternal());
        }

        private async void CreateAccountInternal()
        {
            try
            {
                ButtonSwitcher.Enabled = false;

                var login = UserModel.Login.SecureStringToString();
                var email = UserModel.Email.SecureStringToString();
                var pass1 = UserModel.Password.SecureStringToString();
                var pass2 = UserModel.ConfirmPassword.SecureStringToString();

                foreach (var data in new[] { login, email, pass1, pass2 })
                {
                    if (data.IsEmpty())
                    {
                        _messanger.Go("All fields marked with a star must be filled.", "Check required data fields");

                        return;
                    }
                }
                if (pass1 != pass2)
                {
                    _messanger.Go("Passwords must match each other.", "Check your password");

                    return;
                }
                else
                {
                    var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
                    while (ApplicationContext.NotInitialized)
                    {
                        Thread.Sleep(0);
                    }

                    var userRepository = container.GetRepository<User>();
                    var verifiedUser1 = await userRepository.FindAsync(u => u.Login == login);
                    if (verifiedUser1 != null)
                    {
                        _messanger.Go("User with same login already exist.", "Not suitable login");

                        return;
                    }
                    var verifiedUser2 = await userRepository.FindAsync(u => u.Email == email);
                    if (verifiedUser2 != null)
                    {
                        _messanger.Go("User with same email already exist.", "Not suitable email");

                        return;
                    }

                    // SetRole Here:
                    // ...
                    // if (access switched Admin) 1)
                    // {
                    //     ... todo
                    // }
                    // else if (access switched User) 2)
                    // {
                    //     ... todo
                    // }

                    var access = _registrationRole == RegistrationRole.User ? RoleDefinition.User : RoleDefinition.Admin;
                    var role = await container.GetRepository<Role>().FindAsync(r => r.Access == access);
                    var user = new User
                    {
                        Name = UserModel.Name,
                        Surname = UserModel.Surname,

                        Login = login,
                        Email = email,
                        Password = pass1,
                        RoleId = role.Id
                    };
                    await userRepository.AddAsync(user);

                    var securePassword = user.Password.StringToSecureString();

                    // 1):
                    // ... todo

                    // 2):
                    base.MarshalAction(() =>
                    {
                        var context = _navigation.NavigateToPageAndGetContext<ContactsPage, ContactsPageViewModel>();
                        context.OwnerModel = new UserModel
                        {
                            Name = user.Name,
                            Surname = user.Name,
                            Role = user.Role.Access,

                            Login = UserModel.Login,
                            Email = UserModel.Email,

                            Password = securePassword,
                            ConfirmPassword = securePassword,

                            ImageModel = new ImageModel(File.ReadAllBytes(@"Assets/default_M_user.png"))
                        };

                        _resourceService.SetUserResource(context.OwnerModel);
                    });
                }
            }
            finally
            {
                ButtonSwitcher.Enabled = true;
                ProgressAccess.ChangeAccess();
            }
        }
    }
}