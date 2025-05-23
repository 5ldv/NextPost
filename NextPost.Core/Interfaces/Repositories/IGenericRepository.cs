using NextPost.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Core.Interfaces.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, bool trackEntity, string[]? includes = null);
        Task<IEnumerable<TEntity>> GetAllAsync(bool trackEntity, Expression<Func<TEntity, object>> orderBy = null!,
            string orderDirection = OrderBy.Ascending, string[]? includes = null);
        Task<IEnumerable<TEntity>> GetAllAsync(bool trackEntity, int pageNumber,
            int pageSize, Expression<Func<TEntity, object>> orderBy = null!,
            string orderDirection = OrderBy.Ascending, string[]? includes = null);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
            bool trackEntity, string[]? includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate,
            bool trackEntity, Expression<Func<TEntity, object>> orderBy = null!,
            string orderDirection = OrderBy.Ascending, string[]? includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate,
            bool trackEntity, int pageNumber, int pageSize, Expression<Func<TEntity, object>> orderBy = null!,
            string orderDirection = OrderBy.Ascending, string[]? includes = null);
        Task<TEntity> AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
