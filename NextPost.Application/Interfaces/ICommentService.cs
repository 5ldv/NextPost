using NextPost.Application.DTO_s;

namespace NextPost.Application.Interfaces
{
    public interface ICommentService
    {
        Task AddNewCommentAsync(AddCommentDto dto);
        Task UpdateCommentAsync(UpdateCommentDto dto);
        Task DeleteCommentAsync(int id);
    }
}
