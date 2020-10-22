using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ContactsApp.Domain.Data.Entities;
using ContactsApp.Domain.Data.Repositories;

namespace ContactsApp.Domain.Data.Containers
{
    public interface ISqlContainer : IDisposable
    {
        object GetDataContext();
        int ExecuteSqlQuery(string query);
        Task<int> ExecuteSqlQueryAsync(string query, CancellationToken token = default);
        IQueryable<TEntity> GetFromSqlQuery<TEntity>(string query) where TEntity : class, IBaseEntity;
        ISqlRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity;
    }
}