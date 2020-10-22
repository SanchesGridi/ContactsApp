using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Storage;
using ContactsApp.Infrastructure.Data.Sql.Entities;
using ContactsApp.Infrastructure.Data.Sql.Entities.Configurations;

namespace ContactsApp.Infrastructure.Data.Sql
{
    public class ApplicationContext : DbContext
    {
        private enum InitializationMode : byte
        {
            Async = 0,
            None = 1
        }

        private static bool _notInitialized = true;
        private static InitializationMode _mode = InitializationMode.Async;

        protected internal SqlStorageInfo StorageInfo { get; private set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactSnAccount> ContactSnAccounts { get; set; }
        public DbSet<TelegramAccount> TelegramAccounts { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<ToDoEntry> toDoEntries { get; set; }

        public static bool NotInitialized
        {
            get => _notInitialized;
        }

        public ApplicationContext(SqlStorageInfo info, bool initialize)
        {
            StorageInfo = info;

            if (initialize)
            {
                this.InitializeDatabase().GetAwaiter();
            }
            else
            {
                _notInitialized = false;
            }
        }

        protected internal async Task InitializeDatabase()
        {
            if (_mode == InitializationMode.Async)
            {
                _mode = InitializationMode.None;

                await Task.Run(() =>
                {
                    this.DeleteDatabase();
                    this.CreateDatabase();

                    var initializer = new ApplicationInitializer();
                    initializer.CreateStorageData(this);

                    _notInitialized = false;
                });
            }
        }
        protected internal void DeleteDatabase()
        {
            Database.EnsureDeleted();
        }
        protected internal void CreateDatabase()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new NoteConfiguration());

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLazyLoadingProxies();

            if (StorageInfo.IsMsSql())
            {
                builder.UseSqlServer(StorageInfo.GetStoragePath());
            }
            else if (StorageInfo.IsSqlite())
            {
                builder.UseSqlite(StorageInfo.GetStoragePath());
            }
            else
            {
                throw new ArgumentException();
            }

            base.OnConfiguring(builder);
        }
    }
}