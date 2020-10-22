using System.IO;
using System.Threading;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.Views.Pages;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.IoC.Locators;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.Models.UiModels;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.Infrastructure.Data.Sql;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

// TODO: if navi back SourceCancel()
// TODO: password box binding investigate

namespace ContactsApp.UI.ViewModels
{
    public class LoginPageViewModel : UiMarshalingViewModel
    {
        private UserModel _userModel;
        private SwitchModel _buttonSwitcher;
        private BinAccessModel _progressAccessModel;

        public UserModel UserModel
        {
            get => _userModel ?? (_userModel = new UserModel());
            set => this.SetValue(ref _userModel, value, nameof(UserModel));
        }
        public SwitchModel ButtonSwitcher
        {
            get => _buttonSwitcher ?? (_buttonSwitcher = new SwitchModel(true));
            set => this.SetValue(ref _buttonSwitcher, value, nameof(ButtonSwitcher));
        }
        public BinAccessModel ProgressAccessModel
        {
            get => _progressAccessModel ?? (_progressAccessModel = new BinAccessModel(false));
            set => this.SetValue(ref _progressAccessModel, value, nameof(ProgressAccessModel));
        }
        public ICommand SignInCmd { get; }

        public LoginPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            SignInCmd = new RelayCommand(this.SignInExecute, this.SignInCanExecute);

            base.SubscribeCanExecuteCommand(UserModel, "Email", SignInCmd);
        }

        private bool SignInCanExecute()
        {
            return UserModel.Email.Length > 0 && UserModel.Password.Length > 0;
        }
        private async void SignInExecute()
        {
            await base.MarshalActionAsync(() => ProgressAccessModel.ChangeAccess());
            await base.MarshalActionAsync(() => this.RedirectAccess());
        }

        private async void RedirectAccess()
        {
            try
            {
                ButtonSwitcher.Enabled = false;

                var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
                while (ApplicationContext.NotInitialized)
                {
                    Thread.Sleep(0);
                }

                var email = UserModel.Email.SecureStringToString();
                var pass = UserModel.Password.SecureStringToString();
              
                var userRepository = container.GetRepository<User>();
                var verifiedUser = await userRepository.FindAsync(u => u.Email == email && u.Password == pass);
                if (verifiedUser == null)
                {
                    _messanger.Go("User is not found.", "Error");
                    ProgressAccessModel.ChangeAccess();

                    return;
                }
                else
                {
                    var securePass = verifiedUser.Password.StringToSecureString();

                    if (verifiedUser.Role.IsAdmin())
                    {
                        base.MarshalAction(() =>
                        {
                            var context = _navigation.NavigateToPageAndGetContext<AdminPage, AdminPageViewModel>();
                            context.AdminModel = new UserModel
                            {
                                Name = verifiedUser.Name,
                                Surname = verifiedUser.Surname,
                                Role = verifiedUser.Role.Access,

                                Login = verifiedUser.Login.StringToSecureString(),
                                Email = UserModel.Email,

                                Password = securePass,
                                ConfirmPassword = securePass
                            };
                        });
                    }
                    else
                    {
                        base.MarshalAction(() =>
                        {
                            var context = _navigation.NavigateToPageAndGetContext<ContactsPage, ContactsPageViewModel>();

                            context.OwnerModel = new UserModel
                            {
                                Name = verifiedUser.Name,
                                Surname = verifiedUser.Surname,
                                Role = verifiedUser.Role.Access,

                                Login = verifiedUser.Login.StringToSecureString(),
                                Email = UserModel.Email,

                                Password = securePass,
                                ConfirmPassword = securePass,

                                ImageModel = new ImageModel(verifiedUser.ImageData ?? File.ReadAllBytes(@"Assets/default_M_user.png"))
                            };

                            _resourceService.SetUserResource(context.OwnerModel);

                            foreach (var contact in verifiedUser.Contacts)
                            {
                                var contactModel = new ContactModel(contact.Id)
                                {
                                    Name = contact.Name,
                                    Surname = contact.Surname,
                                    Email = contact.Email,
                                    PhoneNumber = contact.PhoneNumber,
                                    ImageModel = new ImageModel(contact.ImageData ?? File.ReadAllBytes(@"Assets/default_user.png")),
                                };

                                foreach (var account in contact.ContactSnAccounts)
                                {
                                    if (account.IsTelegram())
                                    {
                                        var telegramAccount = account as TelegramAccount;

                                        contactModel.AccountModels.Add(new TelegramAccountModel(telegramAccount.Id)
                                        {
                                            UserName = telegramAccount.UserName,
                                            DisplayUserName = telegramAccount.DisplayUserName,
                                            AccountIdentifierUserName = telegramAccount.AccountIdentifierUserName
                                        });
                                    }
                                }
                                context.Contacts.Add(new ContactSourceViewModel { ContactModel = contactModel });
                            }
                        });
                    }
                    base.UnsubscribeCanExecuteCommand(UserModel, "Email");
                }
            }
            finally
            {
                ButtonSwitcher.Enabled = true;
            }
        }
    }
}