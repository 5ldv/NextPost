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
        Task<TEntity?> GetByIdAsync(int id, bool useAsNoTracking);
        Task<IEnumerable<TEntity>> GetAllAsync(bool useAsNoTracking);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool useAsNoTracking);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool useAsNoTracking);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
