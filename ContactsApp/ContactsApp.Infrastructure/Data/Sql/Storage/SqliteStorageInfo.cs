namespace ContactsApp.Infrastructure.Data.Sql.Storage
{
    public class SqliteStorageInfo : SqlStorageInfo
    {
        public SqliteStorageInfo(string dbName) : base(dbName)
        {
        }

        public override string GetStoragePath()
        {
            return $"Filename={_dbName}";
        }
    }
}