using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NextPost.Application.Dtos;
using NextPost.Application.Helpers;
using NextPost.Application.Interfaces;
using NextPost.Core.Interfaces;
using NextPost.Core.Interfaces.Repositories;
using NextPost.Core.Models;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Services
{
    public class AuthorService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateAuthorDto> updateAuthorDtoValidator,
        IHttpContextAccessor httpContextAccessor
        ) : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<UpdateAuthorDto> _updateAuthorDtoValidator = updateAuthorDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<AuthorDto> GetAuthorByIdAsync(int id)
        {
            if(id <= 0)
                throw new KeyNotFoundException("Author id is not valid");

            var author = await _unitOfWork.Authors.GetByIdAsync(id, false, new[] { "User"});

            if(author is null)
                throw new KeyNotFoundException("Author not found");

            var authorDto = _mapper.Map<AuthorDto>(author);

            return authorDto;
        }
        public async Task<AuthorDto> GetAuthorByUsernameAsync(string username)
        {
            var author = await _unitOfWork.Authors.FindAsync(x => x.User.UserName == username, false, new[] { "User" });
            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }
        public async Task UpdateAuthorAsync(UpdateAuthorDto dto)
        {
            var validationResult = await _updateAuthorDtoValidator.ValidateAsync(dto);
            
            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);

            var user = await _unitOfWork.Users.GetByIdAsync(userId, true, new[] { "Author" });

            if(user is null)
                throw new KeyNotFoundException($"Author with User Id {userId} was not found.");

            if(user.Author is null)
                throw new KeyNotFoundException($"Author with User Id {userId} was not found.");

            _mapper.Map(dto, user.Author);

            await _unitOfWork.SaveChangesAsync();
        }

    }
}
