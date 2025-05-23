using FluentValidation;
using NextPost.Application.DTO_s;
using NextPost.Core.Interfaces;

namespace NextPost.Application.Validators.Comment
{
    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentDtoValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.CommentId)
                .MustAsync(BeExistedComment)
                .WithMessage("Comment does not exist.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(2).WithMessage("Content must be at least 2 characters long.")
                .MaximumLength(400).WithMessage("Content must not exceed 400 characters.");
            _unitOfWork = unitOfWork;
        }

        private async Task<bool> BeExistedComment(int commentId, CancellationToken _) => 
            await _unitOfWork.Comments.IsCommentExists(commentId);

    }


}
