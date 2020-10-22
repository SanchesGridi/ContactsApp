using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models.UiModels
{
    public class SwitchModel : NotificationModel
    {
        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set => this.SetValue(ref _enabled, value, nameof(Enabled));
        }

        public SwitchModel(bool enabled)
        {
            Enabled = enabled;
        }
    }
}