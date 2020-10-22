using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.Views.Pages;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.IoC.Locators;
using ContactsApp.UI.Views.Windows;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.UI.ViewModels
{
    public class ContactDefaultPageViewModel : UiMarshalingViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IWindowStarter _windowStarter;
        private readonly DataTracker _dataTracker;

        private UserModel _user;
        private ObservableCollection<NoteSourceViewModel> _notes;

        public UserModel User
        {
            get => _user ?? (_user = _resourceService.GetUserResource());
            set => this.SetValue(ref _user, value, nameof(User));
        }
        private ObservableCollection<NoteSourceViewModel> Notes
        {
            get => _notes ?? (_notes = new ObservableCollection<NoteSourceViewModel>(_resourceService.GetUserNotesResource())); // TODO: make enter point
            set => this.SetValue(ref _notes, value, nameof(Notes));
        }

        public ICommand LogOutCmd { get; }
        public ICommand LoadImageCmd { get; }
        public ICommand TextChangedCmd { get; }
        public ICommand SaveUserAtCloseCmd { get; }

        public ICommand AddNoteCmd { get; }
        public ICommand RemoveNoteCmd { get; }
        public ICommand EditNoteCmd { get; }
        public ICommand ViewNoteCmd { get; }

        public ContactDefaultPageViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService,
            IDialogService dialogService,
            IWindowStarter windowStarter)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            _dialogService = dialogService;
            _windowStarter = windowStarter;
            _dataTracker = new DataTracker(new[] { "LoginData", "NameData", "SurnameData"});

            LogOutCmd = new RelayCommand(this.LogOutExecute);
            LoadImageCmd = new RelayCommand(this.LoadImageExecute);
            TextChangedCmd = new RelayCommand<string>(this.TextChangedExecute);
            SaveUserAtCloseCmd = new RelayCommand(this.SaveUserAtCloseExecute);

            AddNoteCmd = new RelayCommand(this.AddNoteExecute);
            RemoveNoteCmd = new RelayCommand(this.RemoveNoteExecute);
            EditNoteCmd = new RelayCommand(this.EditNoteExecute);
            ViewNoteCmd = new RelayCommand(this.ViewNoteExecute);
        }

        private void LogOutExecute()
        {
            _resourceService.ResetUserResource();
            _navigation.NavigateToPage<EnterPage>();
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
                    await User.ImageModel.InitilizeSourceAsync(data);

                    var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
                    var userRepository = container.GetRepository<User>();

                    var user = await userRepository.FindAsync(x => x.Email == User.Email.SecureStringToString());
                    user.ImageData = data;
                    userRepository.Update(user);
                }
                base.MarshalTaskFunction(taskFunc, bytes);
            }
        }
        private void TextChangedExecute(string dataName)
        {
            _dataTracker.ChangeData(dataName);
        }
        private async void SaveUserAtCloseExecute()
        {
            var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
            var userRepository = container.GetRepository<User>();

            var user = await userRepository.FindAsync(x => x.Email == User.Email.SecureStringToString());

            if (_dataTracker.IsDataChanged("LoginData"))
            {
                user.Login = User.Login.SecureStringToString();
            }
            if (_dataTracker.IsDataChanged("NameData"))
            {
                user.Name = User.Name;
            }
            if (_dataTracker.IsDataChanged("SurnameData"))
            {
                user.Surname = user.Surname;
            }
            if (_dataTracker.IsUpdateRequired)
            {
                userRepository.Update(user);
            }
        }

        private void RemoveNoteExecute() // TODO
        {

        }
        private void AddNoteExecute()
        {
            var noteData = _windowStarter.StartDialogAndGetContext<NoteWindow, NoteWindowViewModel>(title: "New Note");
        }
        private void EditNoteExecute() // TODO
        {

        }
        private void ViewNoteExecute() // TODO
        {

        }
    }
}