using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NextPost.Api.Configurations;
using NextPost.Application.Dtos;
using NextPost.Application.Helpers;
using NextPost.Application.Interfaces;
using NextPost.Core.Models;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Services
{
    public class JwtTokenService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<AppUser> userManager) : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<string> CreateAccessTokenAsync(AppUser user)
        {
            if(user is null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            var claims = await ClaimsHelper.GetClaimsListAsync(user, _userManager);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
        public async Task<RefreshToken?> RefreshTokenAsync(string refreshToken)
        {
            if(string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException(nameof(refreshToken), "Refresh token cannot be null or empty");


            var tokenOwner = _userManager.Users
            .SingleOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

            if(tokenOwner is null)
                throw new UnauthorizedAccessException("Invalid refresh token");


            var matchedRefreshToken = tokenOwner.RefreshTokens.Single(rt => rt.Token == refreshToken);

            if(!matchedRefreshToken.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token");


            matchedRefreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();

            newRefreshToken.CreatedOn = DateTime.UtcNow;
            newRefreshToken.ExpiresOn.AddDays(_jwtSettings.RefreshTokenExpiryDays);

            tokenOwner.RefreshTokens.Add(newRefreshToken);
            var result = await _userManager.UpdateAsync(tokenOwner);

            if(!result.Succeeded)
                throw new ApplicationException("Failed to update user with new refresh token.");

            return newRefreshToken;
        }
        public RefreshToken CreateRefreshToken()
        {
            var createdRefreshToken = GenerateRefreshToken();
            createdRefreshToken.CreatedOn = DateTime.UtcNow;
            createdRefreshToken.ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

            return createdRefreshToken;
        }
        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            if(string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException(nameof(refreshToken), "Refresh token cannot be null or empty");


            var tokenOwner = _userManager.Users
                .FirstOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

            if(tokenOwner is null)
                throw new UnauthorizedAccessException("Invalid refresh token");


            var matchedRefreshToken = tokenOwner.RefreshTokens.Single(rt => rt.Token == refreshToken);

            if(!matchedRefreshToken.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token");

            matchedRefreshToken.RevokedOn = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(tokenOwner);

            if(!result.Succeeded)
                throw new ApplicationException("Failed to revoke refresh token.");

            return true;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            };
        }

    }
}
