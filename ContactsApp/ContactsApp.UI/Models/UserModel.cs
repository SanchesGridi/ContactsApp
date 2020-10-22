using System.Security;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.Models.UiModels;

namespace ContactsApp.UI.Models
{
    public class UserModel : NotificationReflectorModel
    {
        private string _name;
        private string _surname;
        private string _role;
        private SecureString _login;
        private SecureString _password;
        private SecureString _confirmPassword;
        private SecureString _email;
        private ImageModel _imageModel;

        public string Name
        {
            get => _name;
            set => this.SetWithCommandStateUpdate(ref _name, value, nameof(Name));
        }
        public string Surname
        {
            get => _surname;
            set => this.SetWithCommandStateUpdate(ref _surname, value, nameof(Surname));
        }
        public string Role
        {
            get => _role;
            set => this.SetWithCommandStateUpdate(ref _role, value, nameof(Role));
        }
        public SecureString Login
        {
            get => _login ?? (_login = new SecureString());
            set => this.SetWithCommandStateUpdate(ref _login, value, nameof(Login));
        }
        public SecureString Password
        {
            get => _password ?? (_password = new SecureString());
            set => this.SetWithCommandStateUpdate(ref _password, value, nameof(Password));
        }
        public SecureString ConfirmPassword
        {
            get => _confirmPassword ?? (_confirmPassword = new SecureString());
            set => this.SetWithCommandStateUpdate(ref _confirmPassword, value, nameof(ConfirmPassword));
        }
        public SecureString Email
        {
            get => _email ?? (_email = new SecureString());
            set => this.SetWithCommandStateUpdate(ref _email, value, nameof(Email));
        }
        public ImageModel ImageModel
        {
            get => _imageModel ?? (_imageModel = new ImageModel());
            set => this.SetWithCommandStateUpdate(ref _imageModel, value, nameof(ImageModel));
        }

        public UserModel()
        {
            Reflector = new ModelReflectElement(this);
        }
    }
}