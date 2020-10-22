using System.Windows.Media;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using ContactsApp.UI.Models;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.ViewModels
{
	public class ContactSourceViewModel : ViewModelBase
	{
		private ContactModel _contactModel;

		public ContactModel ContactModel
		{
			get => _contactModel ?? (_contactModel = new ContactModel());
			set => this.SetValue(ref _contactModel, value, nameof(ContactModel));
		}
		public ImageSource ImageSource
		{
			get => ContactModel.ImageModel.ImageSource;
			set { ContactModel.ImageModel.ImageSource = value; base.RaisePropertyChanged(); }
		}
		public string Name
		{
			get => ContactModel.Name;
			set { ContactModel.Name = value; base.RaisePropertyChanged(); }
		}
		public string Surname
		{
			get => ContactModel.Surname;
			set { ContactModel.Surname = value; base.RaisePropertyChanged(); }
		}
		public string PhoneNumber
		{
			get => ContactModel.PhoneNumber;
			set { ContactModel.PhoneNumber = value; base.RaisePropertyChanged(); }
		}
		public string Email
		{
			get => ContactModel.Email;
			set { ContactModel.Email = value; base.RaisePropertyChanged(); }
		}
		public int ContactToken
		{
			get => ContactModel.GetToken();
		}
		public ObservableCollection<TelegramAccountModel> TelegramAccounts
		{
			get => ContactModel.GetTelegramAcoounts();
		}

		public ImageSource PhoneSource
		{
			get => this.GetSource(@"Assets/phone_icon.png");
		}
		public ImageSource EmailSource
		{
			get => this.GetSource(@"Assets/email_icon.png");
		}
		public ImageSource DeleteButtonSource
		{
			get => this.GetSource(@"Assets/delete_image.png");
		}
		public ImageSource TelegramAccountSource
		{
			get => this.GetSource(@"Assets/telegram_icon.png");
		}
		public ImageSource SignalAccountSource
		{
			get => this.GetSource(@"Assets/signal_icon.jpg");
		}
		public ImageSource WireAccountSource 
		{
			get => this.GetSource(@"Assets/wire_icon.png");
		}
	}
}