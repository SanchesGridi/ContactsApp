using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.Infrastructure.Data.Sql.Storage
{
    public sealed class MsSqlStorageInfo : SqlStorageInfo
    {
        private readonly string _engine;

        public MsSqlStorageInfo(string dbName, string engine) : base(dbName)
        {
            _engine = engine.VerifyStringAndSet();
        }

        public override string GetStoragePath()
        {
            return $"Server={_engine};Database={_dbName};Trusted_Connection=True;";
        }
    }
}