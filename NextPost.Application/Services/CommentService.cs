using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NextPost.Application.DTO_s;
using NextPost.Application.Exceptions;
using NextPost.Application.Helpers;
using NextPost.Application.Interfaces;
using NextPost.Core.Interfaces;
using NextPost.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Services
{
    public class CommentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<AddCommentDto> addCommentDtoValidator,
        IValidator<UpdateCommentDto> updateCommentDtoValidator,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CommentService> logger
        ) : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<AddCommentDto> _addPostDtoValidator = addCommentDtoValidator;
        private readonly IValidator<UpdateCommentDto> _updatePostDtoValidator = updateCommentDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<CommentService> _logger = logger;

        public async Task AddNewCommentAsync(AddCommentDto dto)
        {
            _logger.LogInformation("Adding new comment in post with postId: {PostId}", dto.postId);
            var validationResult = await _addPostDtoValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var post = await _unitOfWork.Posts.FindAsync(x => x.Id == dto.postId, false);

            if(post is null || post.IsDeleted)
            {
                throw new PostNotFoundException(dto.postId);
            }

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);

            var author = await _unitOfWork.Authors.FindAsync(x => x.UserId == userId, false);

            if(author is null || author.IsBanned)
            {
                throw new AuthorNotFoundException();
            }

            var comment = new Comment
            {
                Author = author,
                AuthorId = author.Id,
                CreatedAt = DateTime.UtcNow,
                PostId = post.Id,
                Post = post,
                Content = dto.Content,
            };

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("New comment added by authorId: {AuthorId} to postId: {PostId}", author.Id, post.Id);
        }
        public async Task UpdateCommentAsync(UpdateCommentDto dto)
        {
            _logger.LogInformation("Adding new comment with commentId: {CommentId}", dto.CommentId);
            var validationResult = await _updatePostDtoValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);
            var author = await _unitOfWork.Authors.FindAsync(x => x.UserId == userId, false);

            if(author is null || author.IsBanned)
            {
                throw new AuthorNotFoundException();
            }

            var comment = await _unitOfWork.Comments
                .FindAsync(x => x.Id == dto.CommentId && x.AuthorId == author.Id, true);

            if(comment is null || comment.IsDeleted)
            {
                throw new CommentNotFoundException();
            }

            comment.Content = dto.Content;

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Comment updated. commentId: {CommentId}, authorId: {AuthorId}", dto.CommentId, author.Id);
        }
        public async Task DeleteCommentAsync(int commentId)
        {
            _logger.LogInformation("Deleting comment with commentId: {CommentId}", commentId);
            if(commentId <= 0)
            {
                throw new InvalidCommentIdException(commentId);
            }

            var userId = UserContextHelper.GetUserIdFromToken(_httpContextAccessor);
            var author = await _unitOfWork.Authors.FindAsync(x => x.UserId == userId, false);

            if(author is null || author.IsBanned)
            {
                throw new AuthorNotFoundException();
            }

            var comment = await _unitOfWork.Comments
                .FindAsync(x => x.Id == commentId && x.AuthorId == author.Id, true);

            if(comment is null)
            {
                throw new CommentNotFoundException();
            }

            _unitOfWork.Comments.Remove(comment);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Comment deleted. commentId: {CommentId}, authorId: {AuthorId}", commentId, author.Id);
        }
    }
}
