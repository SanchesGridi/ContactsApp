using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.Infrastructure.Data.Sql.Storage
{
    public abstract class SqlStorageInfo
    {
        protected readonly string _dbName;

        public SqlStorageInfo(string dbName)
        {
            _dbName = dbName.VerifyStringAndSet();
        }

        public abstract string GetStoragePath();
    }
}