using NextPost.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> GetPostByIdAsync(int id);
        Task<IEnumerable<PostDto>> GetLastPostedPosts(int pageNumber);
        Task AddNewPostAsync(AddPostDto dto);
        Task UpdatePostAsync(UpdatePostDto dto);
        Task DeletePostAsync(int id);
    }
}
