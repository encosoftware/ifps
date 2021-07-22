using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ENCO.DDD.Repositories
{
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int>
        where TEntity : AggregateRoot
    {
    }

    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

        public abstract IQueryable<TEntity> GetAll();

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAll();
        }

        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public abstract Task<List<TEntity>> GetAllListIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);
        public abstract Task<List<TResult>> GetAllListAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
        public abstract Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);

            return entity ?? throw new EntityNotFoundException(typeof(TEntity), id);
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = GetAll().SingleOrDefault(predicate);

            return entity ?? throw EntityNotFoundException.FromPredicate(typeof(TEntity), predicate);
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefault(predicate);
        }

        public virtual Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(SingleOrDefault(predicate));
        }

        public abstract TEntity SingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);
        public abstract Task<TEntity> SingleIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);
        public abstract TEntity SingleOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);
        public abstract Task<TEntity> SingleOrDefaultIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors);

        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual TEntity Load(TPrimaryKey id)
        {
            return Get(id);
        }

        public abstract TEntity Insert(TEntity entity);        

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        public virtual Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? InsertAsync(entity)
                : UpdateAsync(entity);
        }

        public virtual List<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            return entities.Select(e => Insert(e)).ToList();
        }

        public virtual Task<List<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(entities.Select(e => Insert(e)).ToList());
        }

        public List<TEntity> InsertOrUpdate(IEnumerable<TEntity> entities)
        {
            return entities.Select(e => InsertOrUpdate(e)).ToList();
        }

        public Task<List<TEntity>> InsertOrUpdateAsync(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(entities.Select(e => InsertOrUpdate(e)).ToList());
        }

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        public virtual List<TEntity> Update(IEnumerable<TEntity> entities)
        {
            return entities.Select(e => Update(e)).ToList();
        }

        public virtual Task<List<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(entities.Select(e => Update(e)).ToList());
        }

        public abstract void Delete(TEntity entity);

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }

        public abstract void Delete(TPrimaryKey id);

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.CompletedTask;
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
            return Task.CompletedTask;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        protected abstract IPagedList<TEntity> ToPagedList(IQueryable<TEntity> query, int pageIndex, int pageSize);
        protected abstract Task<IPagedList<TEntity>> ToPagedListAsync(IQueryable<TEntity> query,
            int pageIndex, int pageSize, int indexFrom, CancellationToken cancellationToken);
        protected abstract Task<IPagedList<TResult>> ToPagedListAsync<TResult>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> selector,
            int pageIndex, int pageSize, int indexFrom, CancellationToken cancellationToken);

        public virtual IPagedList<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            IQueryable<TEntity> query = GetAll();

            return GetPagedList(query, predicate, orderBy, pageIndex, pageSize);
        }

        protected virtual IPagedList<TEntity> GetPagedList(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return ToPagedList(orderBy(query), pageIndex, pageSize);
            }
            else
            {
                return ToPagedList(query, pageIndex, pageSize);
            }
        }

        public virtual Task<IPagedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = GetAll();

            return GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize, cancellationToken);
        }

        public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(            
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = GetAll();

            return GetPagedListAsync(query, predicate, selector, orderBy, pageIndex, pageSize, cancellationToken);
        }

        protected virtual Task<IPagedList<TEntity>> GetPagedListAsync(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return ToPagedListAsync(orderBy(query), pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return ToPagedListAsync(query, pageIndex, pageSize, 0, cancellationToken);
            }
        }

        protected virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,             
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return ToPagedListAsync(orderBy(query), selector, pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return ToPagedListAsync(query, selector, pageIndex, pageSize, 0, cancellationToken);
            }
        }
    }
}
