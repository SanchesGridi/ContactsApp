using System.Security;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models
{
    public class GuestModel : NotificationReflectorModel
    {
        private SecureString _email;
        private SecureString _password;

        public SecureString Email
        {
            get => _email ?? (_email = new SecureString());
            set => this.SetWithCommandStateUpdate(ref _email, value, nameof(Email));
        }
        public SecureString Password
        {
            get => _password ?? (_password = new SecureString());
            set => this.SetWithCommandStateUpdate(ref _password, value, nameof(Password));
        }

        public GuestModel()
        {
            Reflector = new ModelReflectElement(this);
        }
    }
}