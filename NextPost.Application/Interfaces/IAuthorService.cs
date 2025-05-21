using NextPost.Application.Dtos;
using NextPost.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetAuthorByUsernameAsync(string username);
        Task<AuthorDto> GetAuthorByIdAsync(int Id);
        Task UpdateAuthorAsync(UpdateAuthorDto dto);
    }
}
