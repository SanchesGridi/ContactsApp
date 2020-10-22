using System.Windows.Input;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.UiServices;
using ContactsApp.UI.IoC.Locators;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.Models.UiModels;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.UI.ViewModels
{
    public class TelegramAccountPageViewModel : UiMarshalingViewModel
    {
		private readonly DataTracker _dataTracker;

		private bool _addRequest;
		private bool _changeRequest;
		private TelegramAccountModel _selectedAccount;
		private ContactSourceViewModel _contactVm;
		private BinAccessModel _saveButtonAccess;
		private BinAccessModel _browsingAccess;
		private BinAccessModel _editAccess;

		public TelegramAccountModel SelectedAccount
		{
			get => _selectedAccount;
			set => this.SetValue(ref _selectedAccount, value, nameof(SelectedAccount));
		}
		public ContactSourceViewModel ContactVm
		{
			get => _contactVm ?? (_contactVm = new ContactSourceViewModel());
			set => this.SetValue(ref _contactVm, value, nameof(ContactVm));
		}
		public BinAccessModel SaveButtonAccess
		{
			get => _saveButtonAccess ?? (_saveButtonAccess = new BinAccessModel(false));
			set => this.SetValue(ref _saveButtonAccess, value, nameof(SaveButtonAccess));
		}
		public BinAccessModel BrowsingAccess
		{
			get => _browsingAccess ?? (_browsingAccess = new BinAccessModel(false));
			set => this.SetValue(ref _browsingAccess, value, nameof(BrowsingAccess));
		}
		public BinAccessModel EditAccess
		{
			get => _editAccess ?? (_editAccess = new BinAccessModel(false));
			set => this.SetValue(ref _editAccess, value, nameof(EditAccess));
		}
		public ICommand AddAccountCmd { get; }
		public ICommand EditAccountCmd { get; }
		public ICommand SaveAccountCmd { get; }
		public ICommand DeleteAccountCmd { get; }
		public ICommand AccountChangedCmd { get; }
		public ICommand TextChangedCmd { get; }

		public TelegramAccountPageViewModel(
			ICommandProvider cmdProvider,
			IUiNavigation navigation,
			IMessageBoxService messanger,
			IObjectSearcher objectSearcher,
			IResourceService resourceService)
			: base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
		{
			_dataTracker = new DataTracker(new[] { "DisplayData", "UserData", "AccountIdentifierData" });

			_addRequest = false;
			_changeRequest = false;

			AddAccountCmd = new RelayCommand(this.AddAccountExecute);
			EditAccountCmd = new RelayCommand(this.EditAccountExecute);
			SaveAccountCmd = new RelayCommand(this.SaveAccountExecute);
			DeleteAccountCmd = new RelayCommand(this.DeleteAccountExecute);
			AccountChangedCmd = new RelayCommand(this.AccountChangedExecute);
			TextChangedCmd = new RelayCommand<string>(this.TextChangedExecute);
		}

		private void AddAccountExecute()
		{
			if (!_addRequest)
			{
				var mainFrame = _objectSearcher.GetLogicalObject<Frame>("OptionsFrame");
				var thisFrame = _objectSearcher.GetLogicalObject<Frame>(mainFrame.Content.AsRef<Page>(), "AccountsFrame");
				var comboBox = _objectSearcher.GetLogicalObject<ComboBox>(thisFrame.Content.AsRef<Page>(), "AccountsBox");
				comboBox.SelectedItem = null;

				SelectedAccount = new TelegramAccountModel();

				_addRequest = true;
				_changeRequest = true;
				this.ChangeUserAccess(false, true);
				SaveButtonAccess.ChangeAccess(true);
			}
		}
		private void EditAccountExecute()
		{
			if (SelectedAccount != null && !_addRequest)
			{
				_changeRequest = true;
				this.ChangeUserAccess(false, true);
				SaveButtonAccess.ChangeAccess(true);
			}
		}
		private async void SaveAccountExecute()
		{
			var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
			var accountRepository = container.GetRepository<TelegramAccount>();

			var account = (TelegramAccount) null;
			var model = SelectedAccount;

			if (_addRequest)
			{
				account = new TelegramAccount();
			}
			else
			{
				account = await accountRepository.FindAsync(model.GetToken());
			}

			if (_dataTracker.IsDataChanged("DisplayData"))
			{
				account.DisplayUserName = model.DisplayUserName;
			}
			if (_dataTracker.IsDataChanged("UserData"))
			{
				account.UserName = model.UserName;
			}
			if (_dataTracker.IsDataChanged("AccountIdentifierData"))
			{
				account.AccountIdentifierUserName = model.AccountIdentifierUserName;
			}

			if (_addRequest)
			{
				_addRequest = false;
				account.OwnerContactId = ContactVm.ContactToken;
				await accountRepository.AddAsync(account);

				base.MarshalAction(() =>
				{
					model.SetToken(account.Id);
					ContactVm.ContactModel.AccountModels.Add(model);
					ContactVm.RaisePropertyChanged("TelegramAccounts");
				});
			}
			else
			{
				if (_dataTracker.IsUpdateRequired)
				{
					accountRepository.Update(account);
					_dataTracker.Reset();
				}
			}

			_changeRequest = false;
			this.ChangeUserAccess(true, false);
			SaveButtonAccess.ChangeAccess(false);
		}
		private void DeleteAccountExecute()
		{
			if (SelectedAccount != null && !_addRequest)
			{
				var container = _resourceService.GetResource<ApplicationLocatorWrapper>("Locator").SqlContainer;
				var accountRepository = container.GetRepository<TelegramAccount>();
				accountRepository.DeleteById(SelectedAccount.GetToken());

				ContactVm.ContactModel.AccountModels.Remove(SelectedAccount);
				ContactVm.RaisePropertyChanged("TelegramAccounts");

				this.ChangeUserAccess(false, false);
			}
		}
		private void AccountChangedExecute()
		{
			_changeRequest = false;

			if (SelectedAccount != null)
			{
				this.ChangeUserAccess(true, false);
				SaveButtonAccess.ChangeAccess(false);
			}
		}
		private void TextChangedExecute(string dataName)
		{
			if (_changeRequest)
			{
				_dataTracker.ChangeData(dataName);
			}
		}

		private void ChangeUserAccess(bool browse, bool edit)
		{
			BrowsingAccess.ChangeAccess(browse);
			EditAccess.ChangeAccess(edit);
		}
	}
}