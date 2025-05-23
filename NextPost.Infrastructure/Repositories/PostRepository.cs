using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NextPost.Core.Interfaces.Repositories;
using NextPost.Core.Models;
using NextPost.Infrastructure.Repository;

namespace NextPost.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<GenericRepository<Post>> _logger;

        public PostRepository(AppDbContext dbContext,
            ILogger<GenericRepository<Post>> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<bool> IsPostExistsAsync(int postId) =>
            await _dbContext.Posts.AnyAsync(x => x.Id == postId);

    }
}
