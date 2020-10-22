using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Domain.Data.Repositories
{
    public interface ISqlRepository<TEntity> where TEntity : class, IBaseEntity
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetWhereIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetAllAsNoTracking();
        IQueryable<TEntity> GetWhereAsNoTracking(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllIncludingAsNoTracking(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetWhereIncludingAsNoTracking(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Find(int id);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(int id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FindIncluding(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity FindIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FindIncludingAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FindIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> FindAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindIncludingAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        void DeleteRange(params TEntity[] entities);
        void Delete(TEntity entity);
        void DeleteById(int id);

        void Update(TEntity entity);
        TEntity Update(TEntity entity, object key);
        void UpdateRange(params TEntity[] entities);

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        bool AddRange(params TEntity[] entities);
        Task<bool> AddRangeAsync(params TEntity[] entities);

        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);

        void Save();
    }
}