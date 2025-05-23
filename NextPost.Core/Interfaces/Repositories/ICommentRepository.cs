using NextPost.Core.Interfaces.Repository;
using NextPost.Core.Models;

namespace NextPost.Core.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<bool> IsCommentExists(int commentId);

    }
}
