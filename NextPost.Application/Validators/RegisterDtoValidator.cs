using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NextPost.Application.Dtos;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterDtoValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(4).WithMessage("Username must be at least 4 characters long.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.")
                .MustAsync(BeAvailableUser).WithMessage("Username is already taken.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MustAsync(BeAvailableEmail).WithMessage("Email is already registered.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }

        private async Task<bool> BeAvailableUser(string username, CancellationToken _)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user == null;
        }

        private async Task<bool> BeAvailableEmail(string email, CancellationToken _)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }
    }
}
