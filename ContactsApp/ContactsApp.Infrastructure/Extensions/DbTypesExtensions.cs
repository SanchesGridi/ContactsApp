using ContactsApp.Infrastructure.Data.Sql.Storage;
using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.Infrastructure.Extensions
{
    public static class DbTypesExtensions
    {
        public static bool IsMsSql(this SqlStorageInfo @this)
        {
            @this.VerifyReferenceInClass();

            return @this is MsSqlStorageInfo;
        }
        public static bool IsSqlite(this SqlStorageInfo @this)
        {
            @this.VerifyReferenceInClass();

            return @this is SqliteStorageInfo;
        }

        public static bool IsAdmin(this Role @this)
        {
            return @this.Access == RoleDefinition.Admin;
        }
        public static bool IsUser(this Role @this)
        {
            return @this.Access == RoleDefinition.User;
        }
        public static bool IsGuest(this Role @this)
        {
            return @this.Access == RoleDefinition.Guest;
        }

        public static bool IsTelegram(this ContactSnAccount @this)
        {
            return @this.GetAccountDomainName() == AccountDefinition.TelegramDomain;
        }
    }
}