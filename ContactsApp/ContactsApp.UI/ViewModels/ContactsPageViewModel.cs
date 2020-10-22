using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.Views.Pages;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;

// TODO: 1)Admin
// TODO: 2)Single App Instance
// TODO: 3)DefaultPage Content: Account; Notes; Maybi create addition page with text only and button for Settings (DefaultPage => Settings(Notes)Page, TextOnluPage => DefaultPage) 
// TODO: 4)SocialAccounts (API's, data for accounts except Telegram)
// TODO: 5)SQLite

namespace ContactsApp.UI.ViewModels
{
    public class ContactsPageViewModel : UiMarshalingViewModel
    {
        private UserModel _ownerModel;
        private ObservableCollection<ContactSourceViewModel> _contacts;

        public UserModel OwnerModel
        {
            get => _ownerModel;
            set => this.SetValue(ref _ownerModel, value, nameof(OwnerModel));
        }
        public ObservableCollection<ContactSourceViewModel> Contacts
        {
            get => _contacts ?? (_contacts = new ObservableCollection<ContactSourceViewModel>());
            set => this.SetValue(ref _contacts, value, nameof(Contacts));
        }
        public bool AddRequest { get; set; }
        public int MyProperty { get; set; }
        public ICommand ContactSelectionChangedCmd { get; }
        public ICommand AddCmd { get; }

        public ContactsPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            ContactSelectionChangedCmd = new RelayCommand<ContactSourceViewModel>(this.ContactSelectionChangedExecute);
            AddCmd = new RelayCommand(this.AddExecute);
        }

        private void ContactSelectionChangedExecute(ContactSourceViewModel selectedContactVm)
        {
            var frame = _objectSearcher.GetLogicalObject<Frame>("OptionsFrame");

            if (selectedContactVm != null)
            {
                var dataPageContext = _navigation.NavigateFromFrameAndGetContext<ContactDataPage, ContactDataPageViewModel>(frame);
                dataPageContext.ContactVm = selectedContactVm;
                dataPageContext.SetParent(this);
            }
            else
            {
                var defaultPageContext = _navigation.NavigateFromFrameAndGetContext<ContactDefaultPage, ContactDefaultPageViewModel>(frame);
                // ...
            }
        }
        private void AddExecute()
        {
            var listBox = _objectSearcher.GetLogicalObject<ListBox>("ContactsBox");
            if (listBox.Items.Count > 0 )
            {
                listBox.SelectedItem = null;
            }

            AddRequest = true;

            var frame = _objectSearcher.GetLogicalObject<Frame>("OptionsFrame");
            var context = _navigation.NavigateFromFrameAndGetContext<ContactDataPage, ContactDataPageViewModel>(frame);
            context.SetParent(this);
            context.ChangeUserAccess();

            async Task taskFunc(byte[] data)
            {
                await context.ContactVm.ContactModel.ImageModel.InitilizeSourceAsync(data);
                context.ContactVm.RaisePropertyChanged("ImageSource");
            }

            base.MarshalTaskFunction(taskFunc, File.ReadAllBytes(@"Assets/default_user.png"));
        }
    }
}