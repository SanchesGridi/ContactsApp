using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.Views.Pages;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.IoC.Locators;
using ContactsApp.UI.Models.UiModels;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.UI.ViewModels
{
    public class ContactDataPageViewModel : UiMarshalingViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly DataTracker _dataTracker;

        private ContactsPageViewModel _parentVm;
        private ContactSourceViewModel _contactVm;
        private BinAccessModel _browsingAccess;
        private BinAccessModel _editAccess;

        public ContactSourceViewModel ContactVm
        {
            get => _contactVm ?? (_contactVm = new ContactSourceViewModel());
            set => this.SetValue(ref _contactVm, value, nameof(ContactVm));
        }
        public BinAccessModel BrowsingAccess
        {
            get => _browsingAccess ?? (_browsingAccess = new BinAccessModel(true));
            set => this.SetValue(ref _browsingAccess, value, nameof(BrowsingAccess));
        }
        public BinAccessModel EditAccess
        {
            get => _editAccess ?? (_editAccess = new BinAccessModel(false));
            set => this.SetValue(ref _editAccess, value, nameof(EditAccess));
        }
        public ICommand DataChangedCmd { get; }
        public ICommand EditContactCmd { get; }
        public ICommand SaveContactCmd { get; }
        public ICommand DeleteContactCmd { get; }
        public ICommand LoadImageCmd { get; }
        public ICommand NavigateToAccountCmd { get; }

        public ContactDataPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService,
            IDialogService dialogService) 
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            _dialogService = dialogService;
            _dataTracker = new DataTracker(new[]
            {
                "NameData", "SurnameData", "PhoneData", "EmailData", "ImageData"
            });

            DataChangedCmd = new RelayCommand<string>(this.DataChangedExecute);
            EditContactCmd = new RelayCommand(this.ChangeUserAccess);
            SaveContactCmd = new RelayCommand(this.SaveContactExecute);
            DeleteContactCmd = new RelayCommand(this.DeleteContactExecute);
            LoadImageCmd = new RelayCommand(this.LoadImageExecute);
            NavigateToAccountCmd = new RelayCommand<string>(this.NavigateToAccountExecute);
        }

        public void SetParent(ContactsPageViewModel parentVm)
        {
            _parentVm = parentVm;
        }
        public void ChangeUserAccess()
        {
            var listBox = _objectSearcher.GetLogicalObject<ListBox>("ContactsBox");
            listBox.IsEnabled = !listBox.IsEnabled;

            BrowsingAccess.ChangeAccess();
            EditAccess.ChangeAccess();
        }

        private void DataChangedExecute(string dataName)
        {
            _dataTracker.ChangeData(dataName);
        }
        private async void SaveContactExecute()
        {
            var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
            var contactRepository = container.GetRepository<Contact>();

            var contact = (Contact)null;
            var model = ContactVm.ContactModel;

            if (_parentVm.AddRequest)
            {
                contact = new Contact();
            }
            else
            {
                contact = await contactRepository.FindAsync(ContactVm.ContactToken);
            }

            if (_dataTracker.IsDataChanged("NameData"))
            {
                contact.Name = model.Name;
            }
            if (_dataTracker.IsDataChanged("SurnameData"))
            {
                contact.Surname = model.Surname;
            }
            if (_dataTracker.IsDataChanged("PhoneData"))
            {
                contact.PhoneNumber = model.PhoneNumber;
            }
            if (_dataTracker.IsDataChanged("EmailData"))
            {
                contact.Email = model.Email;
            }
            if (_dataTracker.IsDataChanged("ImageData"))
            {
                contact.ImageData = model.ImageModel.ImageData;
            }

            if (_parentVm.AddRequest)
            {
                var user = await container.GetRepository<User>().FindAsync(x => x.Email == _parentVm.OwnerModel.Email.SecureStringToString());
                contact.UserId = user.Id;
                await contactRepository.AddAsync(contact);

                base.MarshalAction(() =>
                {
                    model.SetToken(contact.Id);

                    _parentVm.Contacts.Add(ContactVm);
                    _parentVm.AddRequest = false;
                });
            }
            else
            {
                if (_dataTracker.IsUpdateRequired)
                {
                    contactRepository.Update(contact);
                }
            }

            _dataTracker.Reset();

            this.ChangeUserAccess();
        }
        private void DeleteContactExecute()
        {
            if (!_parentVm.AddRequest)
            {
                var result = _messanger.Go("Are you sure?", "Delete Contact Request", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
                    container.GetRepository<Contact>().DeleteById(ContactVm.ContactToken);

                    _parentVm.Contacts.Remove(ContactVm);
                } 
            }
            else
            {
                _parentVm.AddRequest = false;
                this.ChangeUserAccess();

                var frame = _objectSearcher.GetLogicalObject<Frame>("OptionsFrame");
                _navigation.NavigateFromFrame<ContactDefaultPage>(frame);
            }
        }
        private void LoadImageExecute()
        {
            var images = "images (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            var result = _dialogService.OpenFileDialog(images);

            if (result)
            {
                var bytes = File.ReadAllBytes(_dialogService.FilePath);

                async Task taskFunc(byte[] data)
                {
                    await ContactVm.ContactModel.ImageModel.InitilizeSourceAsync(data);
                    ContactVm.RaisePropertyChanged("ImageSource");
                    _dataTracker.ChangeData("ImageData");
                }

                base.MarshalTaskFunction(taskFunc, bytes);
            }
        }
        private void NavigateToAccountExecute(string accountName)
        {
            var mainFrame = _objectSearcher.GetLogicalObject<Frame>("OptionsFrame");
            var thisFrame = _objectSearcher.GetLogicalObject<Frame>(mainFrame.Content.AsRef<Page>(), "AccountsFrame");

            switch (accountName)
            {
                case "Telegram":
                    var context = _navigation.NavigateFromFrameAndGetContext<TelegramAccountPage, TelegramAccountPageViewModel>(thisFrame);
                    context.ContactVm = ContactVm;
                    break;
                case "Signal": // :-)
                    break;
                case "Wire": // :-)
                    break;
            }
        }
    }
}