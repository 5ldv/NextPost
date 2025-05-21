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
        Task<IEnumerable<TEntity>> GetAllAsync(bool trackEntity);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool trackEntity, string[]? includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool trackEntity);
        Task<TEntity> AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
