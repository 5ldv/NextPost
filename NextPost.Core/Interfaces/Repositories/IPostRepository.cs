using NextPost.Core.Interfaces.Repository;
using NextPost.Core.Models;

namespace NextPost.Core.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<bool> IsPostExistsAsync(int postId);
    }
}
