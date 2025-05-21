using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NextPost.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Infrastructure.Repository
{
    public class GenericRepository<TEntity>(
        AppDbContext dbContext,
        ILogger<GenericRepository<TEntity>> logger) : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<GenericRepository<TEntity>> _logger = logger;

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            _logger.LogInformation("Entity of type {EntityType} added successfully.", typeof(TEntity).Name);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool trackEntity)
        {

            IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

            if(!trackEntity)
                query.AsNoTracking();

            var entities = await query.ToListAsync();

            _logger.LogInformation("Found {Count} entities of type {EntityType} matching predicate.",
                entities.Count, typeof(TEntity).Name);

            return query;

        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool trackEntity, string[]? includes = null)
        {

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if(includes is not null)
                foreach(var include in includes)
                    query = query.Include(include);

            if(!trackEntity)
                query.AsNoTracking();

            var entity = await query.FirstOrDefaultAsync(predicate);

            _logger.LogInformation(entity != null
                ? "Entity of type {EntityType} found."
                : "No entity of type {EntityType} found with predicate.", typeof(TEntity).Name);

            return entity;

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackEntity)
        {

            var query = _dbContext.Set<TEntity>();
            if(!trackEntity)
                query.AsNoTracking();

            var entities = await query.ToListAsync();

            _logger.LogInformation("Retrieved all entities of type {EntityType}. Count: {Count}",
                typeof(TEntity).Name, entities.Count);

            return entities;

        }

        public async Task<TEntity?> GetByIdAsync(int id, bool trackEntity, string[]? includes = null)
        {

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if(includes is not null)
                foreach(var include in includes)
                    query = query.Include(include);

            if(!trackEntity)
                query = query.AsNoTracking();

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

            _logger.LogInformation(entity != null
                ? "Entity of type {EntityType} with ID {Id} found."
                : "No entity of type {EntityType} with ID {Id} found.", typeof(TEntity).Name, id);
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _logger.LogInformation("Entity of type {EntityType} removed.", typeof(TEntity).Name);

        }

        public void Update(TEntity entity)
        {

            _dbContext.Set<TEntity>().Update(entity);
            _logger.LogInformation("Entity of type {EntityType} updated.", typeof(TEntity).Name);

        }
    }
}
