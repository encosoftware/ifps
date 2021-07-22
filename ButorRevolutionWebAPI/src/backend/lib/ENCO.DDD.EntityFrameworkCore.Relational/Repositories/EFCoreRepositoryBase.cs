using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ENCO.DDD.EntityFrameworkCore.Relational.Repositories
{
    public abstract class EFCoreRepositoryBase<TDbContext, TEntity> : EFCoreRepositoryBase<TDbContext, TEntity, int>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot<int>
    {
        public EFCoreRepositoryBase(TDbContext context) : base(context)
        {
        }
    }

    public abstract class EFCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot<TPrimaryKey>
    {
        protected readonly TDbContext context;

        private readonly DbSet<TEntity> dbSet;

        protected abstract List<Expression<Func<TEntity, object>>> DefaultIncludes { get; }

        public EFCoreRepositoryBase(TDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = dbSet.AsQueryable();

            if (DefaultIncludes.Any())
            {
                var propertySelectorList = propertySelectors.ToList();
                propertySelectorList.AddRange(DefaultIncludes);
                propertySelectors = propertySelectorList.ToArray();
            }

            if (propertySelectors != null)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public override Task<List<TEntity>> GetAllListAsync()
        {
            return GetAll().ToListAsync();
        }

        public override Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return await GetAllIncluding(propertySelectors).Where(predicate).ToListAsync();
        }

        public override Task<List<TResult>> GetAllListAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Where(predicate).Select(selector).ToListAsync();
        }

        public override Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {
            return GetAll().Where(predicate).Select(selector).SingleAsync();
        }

        public override TEntity Insert(TEntity entity)
        {
            return dbSet.Add(entity).Entity;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override void Delete(TEntity entity)
        {         
            AttachIfNot(entity);
            dbSet.Remove(entity);         
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            //Could not found the entity, do nothing.
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            dbSet.Attach(entity);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        public override Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefaultAsync(predicate);
        }

        public override TEntity SingleIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var entity = SingleOrDefaultIncluding(predicate, propertySelectors);

            return entity ?? throw EntityNotFoundException.FromPredicate(typeof(TEntity), predicate);
        }

        public override async Task<TEntity> SingleIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var entity = await SingleOrDefaultIncludingAsync(predicate, propertySelectors);

            return entity ?? throw EntityNotFoundException.FromPredicate(typeof(TEntity), predicate);
        }

        public override TEntity SingleOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAllIncluding(propertySelectors).SingleOrDefault(predicate);
        }

        public override Task<TEntity> SingleOrDefaultIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAllIncluding(propertySelectors).SingleOrDefaultAsync(predicate);
        }

        protected override IPagedList<TEntity> ToPagedList(IQueryable<TEntity> query, int pageIndex, int pageSize)
        {
            return query.ToPagedList(pageIndex, pageSize);
        }

        protected override Task<IPagedList<TEntity>> ToPagedListAsync(
            IQueryable<TEntity> query, int pageIndex, int pageSize, 
            int indexFrom, CancellationToken cancellationToken)
        {
            return ToPagedListAsync(query, pageIndex, pageSize, 0, cancellationToken);
        }

        protected override Task<IPagedList<TResult>> ToPagedListAsync<TResult>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> selector,
            int pageIndex, int pageSize, int indexFrom, CancellationToken cancellationToken)
        {
            return ToPagedListAsync(query.Select(selector), pageIndex, pageSize, 0, cancellationToken);
        }

        private async Task<IPagedList<T>> ToPagedListAsync<T>(IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }
    }
}
