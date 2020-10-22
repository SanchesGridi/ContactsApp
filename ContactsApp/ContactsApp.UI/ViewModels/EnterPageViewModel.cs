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

// TODO: if navi forward SourceCancel()
// TODO: password box binding investigate

namespace ContactsApp.UI.ViewModels
{
    public class EnterPageViewModel : UiMarshalingViewModel
    {
        private bool _isGuestButtonPressed;

        private BinAccessModel _userAccessModel;
        private BinAccessModel _guestAccessModel;
        private BinAccessModel _progressAccessModel;
        private SwitchModel _guestButtonSwitcher;
        private GuestModel _guestModel;

        public BinAccessModel UserAccessModel
        {
            get => _userAccessModel ?? (_userAccessModel = new BinAccessModel(true));
            set => this.SetValue(ref _userAccessModel, value, nameof(UserAccessModel));
        }
        public BinAccessModel GuestAccessModel
        {
            get => _guestAccessModel ?? (_guestAccessModel = new BinAccessModel(false));
            set => this.SetValue(ref _guestAccessModel, value, nameof(GuestAccessModel));
        }
        public BinAccessModel ProgressAccessModel
        {
            get => _progressAccessModel ?? (_progressAccessModel = new BinAccessModel(false));
            set => this.SetValue(ref _progressAccessModel, value, nameof(ProgressAccessModel));
        }
        public SwitchModel GuestButtonSwitcher
        {
            get => _guestButtonSwitcher ?? (_guestButtonSwitcher = new SwitchModel(true));
            set => this.SetValue(ref _guestButtonSwitcher, value, nameof(GuestButtonSwitcher));
        }
        public GuestModel GuestModel
        {
            get => _guestModel ?? (_guestModel = new GuestModel());
            set => this.SetValue(ref _guestModel, value, nameof(GuestModel));
        }
        public ICommand SignInCmd { get; }
        public ICommand SignUpCmd { get; }
        public ICommand EnterAsGuestCmd { get; }
        public ICommand SignUpAsGuestCmd { get; }
        public ICommand CancelSignUpAsGuestCmd { get; } 

        public EnterPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            SignInCmd = new RelayCommand(this.SignInExecute);
            SignUpCmd = new RelayCommand(this.SignUpExecute);
            EnterAsGuestCmd = new RelayCommand(this.EnterAsGuestExecute);
            SignUpAsGuestCmd = new RelayCommand(this.SignUpAsGuestExecute);
            CancelSignUpAsGuestCmd = new RelayCommand(this.CancelSignUpAsGuestExecute); 
        }

        private void SignUpExecute()
        {
            _navigation.NavigateToPage<RegistrationPage>();
        }
        private void SignInExecute()
        {
            _navigation.NavigateToPage<LoginPage>();
        }
        private void EnterAsGuestExecute()
        {
            this.ChangeUserAccess();
        }
        private async void SignUpAsGuestExecute()
        {
            base.ReinitializeSource();

            await base.MarshalActionAsync(() => ProgressAccessModel.ChangeAccess());
            await base.MarshalActionAsync(() => this.RedirectGuestAccess());
        }
        private void CancelSignUpAsGuestExecute()
        {
            base.SourceCancel();

            this.ChangeUserAccess();
            this.ReleaseGuestRegistrationButton();
        }

        private async void RedirectGuestAccess()
        {
            try
            {
                _isGuestButtonPressed = true;
                GuestButtonSwitcher.Enabled = false;

                var email = GuestModel.Email.SecureStringToString();
                var pass = GuestModel.Password.SecureStringToString();

                if (email.IsEmpty() || pass.IsEmpty())
                {
                    _messanger.Go("All fields must be filled.", "Check required data fields");
                    this.ReleaseGuestRegistrationButton();

                    return;
                }

                var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
                while (ApplicationContext.NotInitialized)
                {
                    Thread.Sleep(0);

                    if (Source.IsCancellationRequested)
                    {
                        return;
                    }
                }

                var userRepository = container.GetRepository<User>();
                var verifiedUser = await userRepository.FindAsync(p => p.Email == email);
                if (verifiedUser != null)
                {
                    _messanger.Go("User with same email already exist.", "Not suitable email");
                    this.ReleaseGuestRegistrationButton();

                    return;
                }
                else
                {
                    if (Source.IsCancellationRequested)
                    {
                        return;
                    }

                    var roleRepository = container.GetRepository<Role>();
                    var role = await roleRepository.FindAsync(u => u.Access == RoleDefinition.Guest);
                    var user = new User
                    {
                        Email = email,
                        Password = pass,
                        RoleId = role.Id
                    };
                    await userRepository.AddAsync(user);

                    var securePassword = user.Password.StringToSecureString();
                    base.MarshalAction(() =>
                    {
                        var context = _navigation.NavigateToPageAndGetContext<ContactsPage, ContactsPageViewModel>();
                        context.OwnerModel = new UserModel
                        {
                            Role = user.Role.Access,

                            Login = user.Login.StringToSecureString(),
                            Email = GuestModel.Email,

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
                GuestButtonSwitcher.Enabled = true;
            }
        }
        private void ReleaseGuestRegistrationButton()
        {
            if (_isGuestButtonPressed)
            {
                ProgressAccessModel.ChangeAccess();
                _isGuestButtonPressed = false;
            }
        }
        private void ChangeUserAccess()
        {
            GuestAccessModel.ChangeAccess();
            UserAccessModel.ChangeAccess();
        }
    }
}