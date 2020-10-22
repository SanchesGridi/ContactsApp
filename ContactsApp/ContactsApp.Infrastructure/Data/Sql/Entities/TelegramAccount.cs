using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public class TelegramAccount : ContactSnAccount
    {
        [NotMapped]
        private readonly string _accountTag;
        [NotMapped]
        private readonly string _accountDomainName;

        public string UserName { get; set; }
        public string DisplayUserName { get; set; }
        public string AccountIdentifierUserName { get; set; }

        public TelegramAccount()
        {
            _accountTag = AccountDefinition.TelegramTag;
            _accountDomainName = AccountDefinition.TelegramDomain;
        }

        public override string GetAccountTag()
        {
            return _accountTag;
        }
        public override string GetAccountDomainName()
        {
            return _accountDomainName;
        }
    }
}