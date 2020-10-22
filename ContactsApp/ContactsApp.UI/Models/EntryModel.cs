using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models
{
    public class EntryModel : NotificationTokenModel
    {
        private string _description;

        public string Description
        {
            get => _description;
            set => this.SetWithCommandStateUpdate(ref _description, value, nameof(Description));
        }

        public EntryModel(int token = NotExistToken) : base(token)
        {
            Reflector = new ModelReflectElement(this);
        }
    }
}