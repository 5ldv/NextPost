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

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _dbContext.AddAsync(entity);
                _logger.LogInformation("Entity of type {EntityType} added successfully.", typeof(TEntity).Name);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding entity of type {EntityType}.", typeof(TEntity).Name);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, bool useAsNoTracking)
        {
            try
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

                if(useAsNoTracking)
                    query.AsNoTracking();

                var entities = await query.ToListAsync();

                _logger.LogInformation("Found {Count} entities of type {EntityType} matching predicate.",
                    entities.Count, typeof(TEntity).Name);

                return query;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error finding entities of type {EntityType} with predicate.",
                    typeof(TEntity).Name);
                throw;
            }
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool useAsNoTracking)
        {
            try
            {
                IQueryable<TEntity> query = _dbContext.Set<TEntity>();

                if(useAsNoTracking)
                    query.AsNoTracking();

                var entity = await query.FirstOrDefaultAsync(predicate);

                _logger.LogInformation(entity != null
                    ? "Entity of type {EntityType} found."
                    : "No entity of type {EntityType} found with predicate.", typeof(TEntity).Name);

                return entity;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error finding entity of type {EntityType} with predicate.",
                    typeof(TEntity).Name);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool useAsNoTracking)
        {
            try
            {
                var query = _dbContext.Set<TEntity>();
                if(useAsNoTracking)
                    query.AsNoTracking();

                var entities = await query.ToListAsync();

                _logger.LogInformation("Retrieved all entities of type {EntityType}. Count: {Count}",
                    typeof(TEntity).Name, entities.Count);

                return entities;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all entities of type {EntityType}.",
                    typeof(TEntity).Name);
                throw;
            }
        }

        public async Task<TEntity?> GetByIdAsync(int id, bool useAsNoTracking)
        {
            try
            {
                TEntity? entity;
                if(useAsNoTracking)
                {
                    entity = await _dbContext.Set<TEntity>()
                        .AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
                }
                else
                {
                    entity = await _dbContext.Set<TEntity>().FindAsync(id);
                }

                _logger.LogInformation(entity != null
                    ? "Entity of type {EntityType} with ID {Id} found."
                    : "No entity of type {EntityType} with ID {Id} found.", typeof(TEntity).Name, id);
                return entity;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity of type {EntityType} with ID {Id}.",
                    typeof(TEntity).Name, id);
                throw;
            }
        }

        public void Remove(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Remove(entity);
                _logger.LogInformation("Entity of type {EntityType} removed.", typeof(TEntity).Name);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error removing entity of type {EntityType}.", typeof(TEntity).Name);
                throw;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Update(entity);
                _logger.LogInformation("Entity of type {EntityType} updated.", typeof(TEntity).Name);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType}.", typeof(TEntity).Name);
                throw;
            }
        }
    }
}
