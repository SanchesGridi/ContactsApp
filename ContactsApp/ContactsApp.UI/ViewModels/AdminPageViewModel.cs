using System.Windows.Input;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;

// TODO

namespace ContactsApp.UI.ViewModels
{
    public class AdminPageViewModel : UiMarshalingViewModel
    {
        private UserModel _adminModel;
        private UserModel _currentUserModel;
        private ObservableCollection<ContactModel> _currentUserContacts;

        public UserModel AdminModel
        {
            get => _adminModel;
            set => this.SetValue(ref _adminModel, value, nameof(AdminModel));
        }
        public UserModel CurrentUserModel
        {
            get => _currentUserModel;
            set => this.SetValue(ref _currentUserModel, value, nameof(CurrentUserModel));
        }
        public ObservableCollection<ContactModel> CurrentUserContacts
        {
            get => _currentUserContacts ?? (_currentUserContacts = new ObservableCollection<ContactModel>());
            set => this.SetValue(ref _currentUserContacts, value, nameof(CurrentUserContacts));
        }
        public ICommand Cmd { get; set; }

        public AdminPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            Cmd = new RelayCommand(this.Execute);
        }

        private void Execute()
        {

        }
    }
}