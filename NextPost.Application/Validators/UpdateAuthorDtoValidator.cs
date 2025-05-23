using FluentValidation;
using NextPost.Application.Dtos;

namespace NextPost.Application.Validators
{
    public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(256).WithMessage("Bio must not exceed 256 characters.");

            RuleFor(x => x.BirthDate)
                .Must(x => x == null || x.Value <= DateTime.UtcNow.AddYears(-12))
                .WithMessage("You must be at least 12 years old.")
                .Must(birthDate => birthDate == null || birthDate.Value > DateTime.UtcNow.AddYears(-100))
                .WithMessage("Birth date must be within the last 100 years");

            RuleFor(x => x.Location)
                .MaximumLength(32).WithMessage("Location must not exceed 32 characters.");
        }
    }



}
