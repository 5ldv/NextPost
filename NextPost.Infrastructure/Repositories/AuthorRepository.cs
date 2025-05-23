using Microsoft.Extensions.Logging;
using NextPost.Core.Interfaces.Repositories;
using NextPost.Core.Models;
using NextPost.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Infrastructure.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<GenericRepository<Author>> _logger;

        public AuthorRepository(AppDbContext dbContext,
            ILogger<GenericRepository<Author>> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}
