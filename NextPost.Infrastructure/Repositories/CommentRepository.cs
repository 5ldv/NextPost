using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NextPost.Core.Interfaces.Repositories;
using NextPost.Core.Models;
using NextPost.Infrastructure.Repository;

namespace NextPost.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<GenericRepository<Comment>> _logger;

        public CommentRepository(AppDbContext dbContext,
            ILogger<GenericRepository<Comment>> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> IsCommentExists(int commentId) => 
            await _dbContext.Comments.AnyAsync(x => x.Id == commentId);
        
    }
}
