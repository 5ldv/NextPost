using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NextPost.Application.Dtos;
using NextPost.Application.Exceptions;
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
        IHttpContextAccessor httpContextAccessor,
        ILogger<IAuthorService> logger
        ) : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<UpdateAuthorDto> _updateAuthorDtoValidator = updateAuthorDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<IAuthorService> _logger = logger;

        public async Task<AuthorDto> GetAuthorByIdAsync(int id)
        {
            _logger.LogInformation("Getting author by id: {AuthorId}", id);
            if(id <= 0)
            {
                throw new InvalidAuthorIdException(id);
            }

            var author = await _unitOfWork.Authors.GetByIdAsync(id, false, new[] { "User" });

            if(author is null)
            {
                throw new AuthorNotFoundException(id);
            }

            var authorDto = _mapper.Map<AuthorDto>(author);

            _logger.LogInformation("Successfully retrieved author for id: {AuthorId}", id);
            return authorDto;
        }
        public async Task<AuthorDto> GetAuthorByUsernameAsync(string username)
        {
            _logger.LogInformation("Getting author by username: {Username}", username);
            var author = await _unitOfWork.Authors
                .FindAsync(x => x.User.UserName == username, false, new[] { "User" });

            if(author is null)
            {
                throw new AuthorNotFoundException();
            }

            var authorDto = _mapper.Map<AuthorDto>(author);
            _logger.LogInformation("Successfully retrieved author for username: {Username}", username);
            return authorDto;
        }
        public async Task UpdateAuthorAsync(UpdateAuthorDto dto)
        {
            _logger.LogInformation("Updating author with data: {@UpdateAuthorDto}", dto);
            var validationResult = await _updateAuthorDtoValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);

            var user = await _unitOfWork.Users.GetByIdAsync(userId, true, new[] { "Author" });

            if(user is null)
            {
                throw new AuthorNotFoundException();
            }

            if(user.Author is null)
            {
                throw new AuthorNotFoundException();
            }

            _mapper.Map(dto, user.Author);

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Successfully updated author for user id: {UserId}", userId);
        }

    }
}
