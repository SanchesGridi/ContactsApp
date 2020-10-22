using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using ContactsApp.Domain.Data.Entities;
using ContactsApp.Domain.Data.Repositories;

namespace ContactsApp.Infrastructure.Data.Sql
{
    public class SqlRepository<TEntity> : ISqlRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected DbContext _context;
        protected DbSet<TEntity> _entitySet;

        protected DbContext Context
        {
            get => _context;
            private set => _context = value;
        }
        protected DbSet<TEntity> EntitySet
        {
            get => _entitySet;
            private set => _entitySet = value;
        }

        public SqlRepository(DbContext context)
        {
            Context = context;
            EntitySet = context.Set<TEntity>();
        }

        // Get methods
        public virtual IQueryable<TEntity> GetAll()
        {
            return EntitySet;
        }
        public virtual IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitySet.Where(predicate);
        }
        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.QueryableWithIncludeProperties(EntitySet, includeProperties);
        }
        public virtual IQueryable<TEntity> GetWhereIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.QueryableWithIncludeProperties(EntitySet.Where(predicate), includeProperties);
        }

        // Get AsNoTracking methods
        public virtual IQueryable<TEntity> GetAllAsNoTracking()
        {
            return EntitySet.AsNoTracking();
        }
        public virtual IQueryable<TEntity> GetWhereAsNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitySet.Where(predicate).AsNoTracking();
        }
        public virtual IQueryable<TEntity> GetAllIncludingAsNoTracking(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.GetAllIncluding(includeProperties).AsNoTracking();
        }
        public virtual IQueryable<TEntity> GetWhereIncludingAsNoTracking(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.GetWhereIncluding(predicate, includeProperties).AsNoTracking();
        }

        // Find methods
        public virtual TEntity Find(int id) // warning exception!
        {
            var entity = EntitySet.Find(id);

            return entity ?? throw new NullReferenceException("Searched entity not found.");
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate) // warning exception!
        {
            return EntitySet.SingleOrDefault(predicate);
        }
        public virtual async Task<TEntity> FindAsync(int id) // warning exception!
        {
            var entity = await EntitySet.FindAsync(id).AsTask();

            return entity ?? throw new NullReferenceException("Searched entity not found.");
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate) // warning exception!
        {
            return await EntitySet.SingleOrDefaultAsync(predicate);
        }
        public virtual TEntity FindIncluding(int id, params Expression<Func<TEntity, object>>[] includeProperties) // warning exception!
        {
            var queryable = EntitySet.Where(entity => entity.Id == id);
            return this.QueryableWithIncludeProperties(queryable, includeProperties).SingleOrDefault();
        }
        public virtual TEntity FindIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) // warning exception!
        {
            var queryable = EntitySet.Where(predicate);
            return this.QueryableWithIncludeProperties(queryable, includeProperties).SingleOrDefault();
        }
        public virtual async Task<TEntity> FindIncludingAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties) // warning exception!
        {
            var queryable = EntitySet.Where(entity => entity.Id == id);
            return await this.QueryableWithIncludeProperties(queryable, includeProperties).SingleOrDefaultAsync();
        }
        public virtual async Task<TEntity> FindIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) // warning exception!
        {
            var queryble = EntitySet.Where(predicate);
            return await this.QueryableWithIncludeProperties(queryble, includeProperties).SingleOrDefaultAsync();
        }

        // Find AsNoTracking methods
        public virtual async Task<TEntity> FindAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate) // warning exception!
        {
            return await EntitySet.Where(predicate).AsNoTracking().SingleOrDefaultAsync();
        }
        public virtual async Task<TEntity> FindIncludingAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) // warning exception!
        {
            var queryble = EntitySet.Where(predicate);
            return await this.QueryableWithIncludeProperties(queryble, includeProperties).AsNoTracking().SingleOrDefaultAsync();
        }

        // Delete methods
        public void DeleteRange(params TEntity[] entities)
        {
            if (entities != null)
            {
                EntitySet.RemoveRange(entities);
                this.Save();
            }
        }
        public virtual void Delete(TEntity entity)
        {
            if (entity != null)
            {
                EntitySet.Remove(entity);
                this.Save();
            }
            else
            {
                return;
            }
        }
        public virtual void DeleteById(int id)
        {
            var entity = this.Find(id);
            if (entity != null)
            {
                EntitySet.Remove(entity);
                this.Save();
            }
            else
            {
                return;
            }
        }

        // Update methods
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            else
            {
                Context.Entry(entity).State = EntityState.Modified;
                this.Save();
            }
        }
        public virtual TEntity Update(TEntity entity, object key)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                var existEntity = EntitySet.Find(key);
                if (existEntity != null)
                {
                    Context.Entry(existEntity).CurrentValues.SetValues(entity);
                    this.Save();
                    return existEntity;
                }
                else
                {
                    return null;
                }
            }
        }
        public void UpdateRange(params TEntity[] entities)
        {
            if (entities != null)
            {
                EntitySet.UpdateRange(entities);
                this.Save();
            }
        }

        // Add methods
        public virtual TEntity Add(TEntity entity)
        {
            return entity != null ? this.AddInternal(entity) : throw new ArgumentNullException(nameof(entity), "Check entity argument");
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            return entity != null ? await this.AddInternalAsync(entity) : throw new ArgumentNullException(nameof(entity), "Check entity argument");
        }
        public virtual bool AddRange(params TEntity[] entities)
        {
            if (entities != null)
            {
                EntitySet.AddRange(entities);
                this.Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual async Task<bool> AddRangeAsync(params TEntity[] entities)
        {
            if (entities != null)
            {
                await EntitySet.AddRangeAsync(entities);
                this.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Count methods
        public virtual int Count()
        {
            return EntitySet.Count();
        }
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitySet.Count(predicate);
        }

        // Save methods
        public virtual void Save()
        {
            Context.SaveChanges();
        }

        // Internal methods
        private IQueryable<TEntity> QueryableWithIncludeProperties(IQueryable<TEntity> queryable, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }
            return queryable;
        }
        private TEntity AddInternal(TEntity entity)
        {
            EntitySet.Add(entity);
            this.Save();
            return entity;
        }
        private async Task<TEntity> AddInternalAsync(TEntity entity)
        {
            await EntitySet.AddAsync(entity);
            this.Save();
            return entity;
        }
    }
}