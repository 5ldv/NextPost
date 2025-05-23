using FluentValidation;
using NextPost.Application.DTO_s;

namespace NextPost.Application.Validators.Comment
{
    public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
    {
        public AddCommentDtoValidator()
        {

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(2).WithMessage("Content must be at least 2 characters long.")
                .MaximumLength(400).WithMessage("Content must not exceed 400 characters.");
        }
    }


}
