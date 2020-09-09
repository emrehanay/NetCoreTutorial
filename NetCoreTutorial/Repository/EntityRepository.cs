using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NetCoreTutorial.Helpers;

namespace NetCoreTutorial.Repository
{
    public abstract class EntityRepository<TEntity, TContext> : IEntityRepository<TEntity>, IUnitOfWorkRepository
        where TEntity : class
        where TContext : DbContext
    {
        private protected readonly TContext Context;
        private readonly DbSet<TEntity> _dbSet;

        protected EntityRepository(TContext context)
        {
            Context = context;
            _dbSet = Context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Expression<Func<TEntity, TEntity>> selectExpression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            if (selectExpression != null) query = query.Select(selectExpression);
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Expression<Func<TEntity, TEntity>> selectExpression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            if (selectExpression != null) query = query.Select(selectExpression);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Expression<Func<TEntity, TEntity>> selectExpression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            if (selectExpression != null) query = query.Select(selectExpression);
            return await query.FirstOrDefaultAsync();
        }

        public async Task Add(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> AddAndSave(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task AddAndSaveRange(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Remove(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public DbContext GetDbContext()
        {
            return Context;
        }

        public async Task<PagedResult<TEntity>> GetPaged(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Expression<Func<TEntity, TEntity>> selectExpression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int page = 1, int pageSize = 20)
        {
            IQueryable<TEntity> query = _dbSet;
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            if (selectExpression != null) query = query.Select(selectExpression);

            var count = await query.CountAsync();

            var result = new PagedResult<TEntity> {CurrentPage = page, PageSize = pageSize, RowCount = count};

            var pageCount = (double) result.RowCount / pageSize;
            result.PageCount = (int) Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.TempResult = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<long> GetCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null) query = query.Where(predicate);
            return await query.CountAsync();
        }
    }
}