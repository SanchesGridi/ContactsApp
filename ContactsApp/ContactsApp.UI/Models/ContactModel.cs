using System.Collections.ObjectModel;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.Models.UiModels;

namespace ContactsApp.UI.Models
{
    public class ContactModel : NotificationTokenModel
    {
        private string _name;
        private string _surname;
        private string _phoneNumber;
        private string _email;
        private ImageModel _imageModel;
        private ObservableCollection<ISocialAccountModel> _accountModels;

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
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => this.SetWithCommandStateUpdate(ref _phoneNumber, value, nameof(PhoneNumber));
        }
        public string Email
        {
            get => _email;
            set => this.SetWithCommandStateUpdate(ref _email, value, nameof(Email));
        }
        public ImageModel ImageModel
        {
            get => _imageModel ?? (_imageModel = new ImageModel());
            set => this.SetWithCommandStateUpdate(ref _imageModel, value, nameof(ImageModel));
        }
        public ObservableCollection<ISocialAccountModel> AccountModels
        {
            get => _accountModels ?? (_accountModels = new ObservableCollection<ISocialAccountModel>());
            set => this.SetWithCommandStateUpdate(ref _accountModels, value, nameof(AccountModels));
        }

        public ContactModel(int token = NotExistToken) : base(token)
        {
            Reflector = new ModelReflectElement(this);
        }
    }
}