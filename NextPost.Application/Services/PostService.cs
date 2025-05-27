using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NextPost.Application.Constants;
using NextPost.Application.DTO_s;
using NextPost.Application.Dtos;
using NextPost.Application.Exceptions;
using NextPost.Application.Helpers;
using NextPost.Application.Interfaces;
using NextPost.Core.Interfaces;
using NextPost.Core.Models;
using NextPost.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Services
{
    public class PostService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<AddPostDto> addPostDtoValidator,
        IValidator<UpdatePostDto> updatePostDtoValidator,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PostService> logger) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<AddPostDto> _addPostDtoValidator = addPostDtoValidator;
        private readonly IValidator<UpdatePostDto> _updatePostDtoValidator = updatePostDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<PostService> _logger = logger;

        public async Task<PostDto> GetPostByIdAsync(int postId)
        {
            _logger.LogInformation("Getting post be post id {postId}", postId);
            if(postId <= 0)
            {
                throw new InvalidPostIdException(postId);
            }

            var post = await _unitOfWork.Posts.FindAsync(x => x.Id == postId, false, new[] { "Author", "Comments" });

            if(post is null || post.IsDeleted)
            {
                throw new PostNotFoundException(postId);
            }

            var postDto = _mapper.Map<PostDto>(post);

            _logger.LogInformation("Post with id {PostId} found successfully", postId);
            return postDto;
        }
        public async Task AddNewPostAsync(AddPostDto dto)
        {
            _logger.LogInformation("Adding new post titled ({title})", dto.Title);

            var validationResult = await _addPostDtoValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var post = _mapper.Map<Post>(dto);

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);
            var author = await _unitOfWork.Authors.FindAsync(x => x.UserId == userId, true);

            if(author is null)
            {
                throw new AuthorNotFoundException();
            }

            post.AuthorId = author.Id;
            post.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Posts.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Post added successfully - titled ({title})", dto.Title);
        }
        public async Task UpdatePostAsync(UpdatePostDto dto)
        {
            _logger.LogInformation("Updating post with id ({[postId]})", dto.postId);

            var validationResult = await _updatePostDtoValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);

            // to check if user is not an author.
            var author = await _unitOfWork.Authors.FindAsync(x => x.UserId == userId, false);

            if(author is null)
                throw new AuthorNotFoundException();

            var post = await _unitOfWork.Posts
                .FindAsync(x => x.Id == dto.postId && x.AuthorId == author.Id, true);

            if(post is null || post.IsDeleted)
                throw new PostNotFoundException(dto.postId);

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Post with id {postId} updated successfully", dto.postId);
        }
        public async Task DeletePostAsync(int postId)
        {
            _logger.LogInformation("Deleting post with id ({[postId]})", postId);

            if(postId <= 0)
                throw new InvalidPostIdException(postId);

            var post = await _unitOfWork.Posts.FindAsync(x => x.Id == postId, true);

            if(post is null)
                throw new PostNotFoundException(postId);

            _unitOfWork.Posts.Remove(post);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Post with id {postId} deleted successfully", postId);

        }

        public async Task<IEnumerable<PostDto>> GetLastPostedPosts(int pageNumber)
        {
            IEnumerable<Post> posts = await _unitOfWork.Posts
                .FindAllAsync(p => !p.IsDeleted, false, pageNumber, PageConstants.PageSize, p => p.CreatedAt,
                OrderBy.Descending, new[] { "Author", "Comments" });

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}
