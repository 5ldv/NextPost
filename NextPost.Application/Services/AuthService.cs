using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextPost.Application.Constants;
using NextPost.Application.Dtos;
using NextPost.Application.Interfaces;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Services
{
    public class AuthService(
        IMapper mapper, IJwtTokenService jwtTokenService,
        UserManager<AppUser> userManager,
        IValidator<RegisterDto> registerDtoValidator,
        IValidator<LoginDto> loginDtoValidator) : IAuthService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IValidator<RegisterDto> _registerDtoValidator = registerDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator = loginDtoValidator;

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);

            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);


            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if(user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new UnauthorizedAccessException("Invalid user or password");


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
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDto);
            
            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);


            var user = _mapper.Map<AppUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
                 throw new ApplicationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            var accessToken = await _jwtTokenService.CreateAccessTokenAsync(user);
            var refreshToken = _jwtTokenService.CreateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
            if(!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                throw new ApplicationException(string.Join("; ", roleResult.Errors.Select(e => e.Description)));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
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
            var newRefreshToken = await _jwtTokenService.RefreshTokenAsync(refreshToken);

            if(newRefreshToken is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == newRefreshToken.Token));

            if(user is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var newAccessToken = await _jwtTokenService.CreateAccessTokenAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

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
        public async Task<bool> RevokeUserToken(string refreshToken)
        {
            return await _jwtTokenService.RevokeRefreshTokenAsync(refreshToken);
        }
    }
}
