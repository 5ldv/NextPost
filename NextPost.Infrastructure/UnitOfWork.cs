using Microsoft.Extensions.Logging;
using NextPost.Core.Interfaces.Repository;
using NextPost.Core.Interfaces;
using NextPost.Core.Models.Identity;
using NextPost.Infrastructure.Repository;
using NextPost.Infrastructure;
using NextPost.Infrastructure.Repositories;
using NextPost.Core.Interfaces.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly ILoggerFactory _loggerFactory;

    public IGenericRepository<AppUser> Users { get; }
    public IGenericRepository<AppRole> Roles { get; }
    public IAuthorRepository Authors { get; }
    public IPostRepository Posts { get; }
    public ICommentRepository Comments { get; }

    public UnitOfWork(AppDbContext dbContext, ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _loggerFactory = loggerFactory;

        Users = new GenericRepository<AppUser>(_dbContext, _loggerFactory.CreateLogger<GenericRepository<AppUser>>());
        Roles = new GenericRepository<AppRole>(_dbContext, _loggerFactory.CreateLogger<GenericRepository<AppRole>>());
        Authors = new AuthorRepository(_dbContext, _loggerFactory.CreateLogger<AuthorRepository>());
        Posts = new PostRepository(_dbContext, _loggerFactory.CreateLogger<PostRepository>());
        Comments = new CommentRepository(_dbContext, _loggerFactory.CreateLogger<CommentRepository>());
    }

    public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    public void Dispose() => _dbContext.Dispose();
}