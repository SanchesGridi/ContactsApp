using System.Windows;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models.UiModels
{
    public sealed class BinAccessModel : NotificationModel
    {
        private bool _access;
        private BinVisibilityModel _visibilityModel;
        private SwitchModel _switchModel;

        private BinVisibilityModel VisibilityModel
        {
            get => _visibilityModel;
            set => this.SetValue(ref _visibilityModel, value, nameof(VisibilityModel));
        }
        private SwitchModel SwitchModel
        {
            get => _switchModel;
            set => this.SetValue(ref _switchModel, value, nameof(SwitchModel));
        }

        public Visibility VisibilityAccess
        {
            get => VisibilityModel.Visibility;
        }
        public bool SwitcherAceess
        {
            get => SwitchModel.Enabled;
        }

        public BinAccessModel(bool startAccess)
        {
            VisibilityModel = new BinVisibilityModel(startAccess);
            SwitchModel = new SwitchModel(startAccess);
            _access = startAccess;
        }
     
        public void ChangeAccess()
        {
            _access = !_access;
            SwitchModel.Enabled = _access;
            VisibilityModel.Visibility = _access ? Visibility.Visible : Visibility.Hidden;

            base.OnPropertyChanged(nameof(VisibilityAccess));
            base.OnPropertyChanged(nameof(SwitcherAceess));
        }
        public void ChangeAccess(bool access)
        {
            _access = access;
            SwitchModel.Enabled = access;
            VisibilityModel.Visibility = _access ? Visibility.Visible : Visibility.Hidden;

            base.OnPropertyChanged(nameof(VisibilityAccess));
            base.OnPropertyChanged(nameof(SwitcherAceess));
        }
    }
}