using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextPost.Application.DTO_s;
using NextPost.Core.Models;
using NextPost.Infrastructure;

namespace NextPost.Application.Validators.Post
{
    public class AddPostDtoValidator : AbstractValidator<AddPostDto>
    {
        public AddPostDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(20).WithMessage("Title must be at least 20 characters long.")
                .MaximumLength(128).WithMessage("Title must not exceed 128 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(200).WithMessage("Content must be at least 200 characters long.")
                .MaximumLength(4000).WithMessage("Content must not exceed 4000 characters.");
        }
    }

}
