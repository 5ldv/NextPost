using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NextPost.Application.Constants;
using NextPost.Application.Dtos;
using NextPost.Application.Exceptions;
using NextPost.Application.Interfaces;
using NextPost.Core.Interfaces;
using NextPost.Core.Models;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NextPost.Application.Services
{
    public class AuthService(
        IMapper mapper, IJwtTokenService jwtTokenService,
        UserManager<AppUser> userManager,
        IValidator<RegisterDto> registerDtoValidator,
        IValidator<LoginDto> loginDtoValidator,
        IUnitOfWork unitOfWork,
        ILogger<AuthService> logger) : IAuthService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IValidator<RegisterDto> _registerDtoValidator = registerDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator = loginDtoValidator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AuthService> _logger = logger;

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if(user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new InvalidCredentialsException();
            }

            var activeRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.IsActive);

            if(activeRefreshToken is not null)
            {
                activeRefreshToken.RevokedOn = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            var accessToken = await _jwtTokenService.CreateAccessTokenAsync(user);
            var refreshToken = _jwtTokenService.CreateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("User {Username} logged in successfully", loginDto.Username);

            return new AuthResponseDto
            {
                Token = accessToken,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = userRoles,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", dto.Username);
            var validationResult = await _registerDtoValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(!result.Succeeded)
            {
                throw new RegistrationFailedException(result.Errors.Select(e => e.Description));
            }

            var author = new Author
            {
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                User = user
            };

            var addedAuthor = await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();

            user.Author = addedAuthor;

            var accessToken = await _jwtTokenService.CreateAccessTokenAsync(user);
            var refreshToken = _jwtTokenService.CreateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Author);
            if(!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new RegistrationFailedException(roleResult.Errors.Select(e => e.Description));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation("User {Username} registered successfully", dto.Username);
            return new AuthResponseDto
            {
                Token = accessToken,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = userRoles,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }
        public async Task<AuthResponseDto> RefreshUserTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Refresh token attempt: {RefreshToken}", refreshToken);
            var newRefreshToken = await _jwtTokenService.RefreshTokenAsync(refreshToken);

            if(newRefreshToken is null)
            {
                throw new InvalidRefreshTokenException();
            }

            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == newRefreshToken.Token));

            if(user is null)
            {
                throw new InvalidRefreshTokenException(refreshToken);
            }

            var newAccessToken = await _jwtTokenService.CreateAccessTokenAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Refresh token succeeded for user: {Username}", user.UserName);

            return new AuthResponseDto
            {
                Token = newAccessToken,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = userRoles,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }
        public async Task<bool> RevokeUserTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Revoke refresh token attempt: {RefreshToken}", refreshToken);

            var result = await _jwtTokenService.RevokeRefreshTokenAsync(refreshToken);

            _logger.LogInformation("Revoke refresh token result for {RefreshToken}: {Result}", refreshToken, result);
            return result;
        }
    }
}
