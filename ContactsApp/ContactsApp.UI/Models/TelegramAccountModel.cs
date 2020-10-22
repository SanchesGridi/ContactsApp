using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.UI.Models
{
    public class TelegramAccountModel : NotificationTokenModel, ISocialAccountModel
    {
        private readonly string _domainName;
        private readonly string _accountTag;

        private string _userName;
        private string _displayUserName;
        private string _accountIdentifierUserName;

        public string UserName
        {
            get => _userName;
            set => this.SetWithCommandStateUpdate(ref _userName, value, nameof(UserName));
        }
        public string DisplayUserName
        {
            get => _displayUserName;
            set => this.SetWithCommandStateUpdate(ref _displayUserName, value, nameof(DisplayUserName));
        }
        public string AccountIdentifierUserName
        {
            get => _accountIdentifierUserName;
            set => this.SetWithCommandStateUpdate(ref _accountIdentifierUserName, value, nameof(AccountIdentifierUserName));
        }

        public TelegramAccountModel(int token = NotExistToken) : base(token)
        {
            _domainName = AccountDefinition.TelegramDomain;
            _accountTag = AccountDefinition.TelegramTag;

            Reflector = new ModelReflectElement(this);
        }

        public string GetAccountTag()
        {
            return _accountTag;
        }
        public string GetDomainName()
        {
            return _domainName;
        }
    }
}