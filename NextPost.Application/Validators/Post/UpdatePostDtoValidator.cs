using FluentValidation;
using NextPost.Application.DTO_s;
using NextPost.Core.Interfaces;

namespace NextPost.Application.Validators.Post
{
    public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePostDtoValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(x => x.postId)
                .MustAsync(BeExistedPost)
                .WithMessage("The specified post does not exist.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(20).WithMessage("Title must be at least 20 characters long.")
                .MaximumLength(128).WithMessage("Title must not exceed 128 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(200).WithMessage("Content must be at least 200 characters long.")
                .MaximumLength(4000).WithMessage("Content must not exceed 4000 characters.");

            _unitOfWork = unitOfWork;
        }
        private async Task<bool> BeExistedPost(int postId, CancellationToken _) => 
            await _unitOfWork.Posts.IsPostExistsAsync(postId);
        
    }

}
