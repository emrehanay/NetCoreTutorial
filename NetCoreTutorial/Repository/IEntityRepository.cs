using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using NetCoreTutorial.Helpers;

namespace NetCoreTutorial.Repository
{
    public interface IEntityRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Expression<Func<T, T>> selectExpression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<T> GetSingle(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Expression<Func<T, T>> selectExpression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<T> GetFirst(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Expression<Func<T, T>> selectExpression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);


        Task Add(T entity);
        Task<T> AddAndSave(T entity);

        Task AddRange(IEnumerable<T> entities);
        Task AddAndSaveRange(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);
        void Remove(IEnumerable<T> entities);

        Task<PagedResult<T>> GetPaged(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Expression<Func<T, T>> selectExpression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int page = 1, int pageSize = 20);

        Task<long> GetCount(Expression<Func<T, bool>> predicate = null);
    }
}