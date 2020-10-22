using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using ContactsApp.Domain.Data.Entities;
using ContactsApp.Domain.Data.Containers;
using ContactsApp.Domain.Data.Repositories;
using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.Infrastructure.Data.Sql
{
    public class SqlContainer : ISqlContainer
    {
        private readonly DbContext _context;
        private readonly Type _repositoryRealizationType;

        private bool _disposed;
        private Dictionary<string, object> _repositories;

        public SqlContainer(DbContext context, Type repositoryRealizationType) 
        {
            repositoryRealizationType.VerifyInterfaceRealization(typeof(ISqlRepository<>));
            repositoryRealizationType.VerifyReference(nameof(repositoryRealizationType));

            _repositoryRealizationType = repositoryRealizationType;
            _context = context.VerifyReferenceAndSet(nameof(context));
            _disposed = false;
        }

        public object GetDataContext()
        {
            return _context;
        }
        public int ExecuteSqlQuery(string query)
        {
            return _context.Database.ExecuteSqlRaw(query);
        }
        public async Task<int> ExecuteSqlQueryAsync(string query, CancellationToken token = default)
        {
            return await _context.Database.ExecuteSqlRawAsync(query, token);
        }
        public IQueryable<TEntity> GetFromSqlQuery<TEntity>(string query) where TEntity : class, IBaseEntity
        {
            return _context.Set<TEntity>().FromSqlRaw(query);
        }
        public ISqlRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var typeName = typeof(TEntity).Name;

            if (_repositories.ContainsKey(typeName))
            {
                return _repositories[typeName] as ISqlRepository<TEntity>;
            }

            var repositoryInstance = Activator.CreateInstance(_repositoryRealizationType.MakeGenericType(typeof(TEntity)), _context);

            _repositories.Add(typeName, repositoryInstance);

            return _repositories[typeName] as ISqlRepository<TEntity>;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories.Clear();
                }

                _context.Dispose();

                _disposed = true;
            }
        }

        ~SqlContainer()
        {
            this.Dispose(false);
        }
    }
}