using NextPost.Application.Dtos;
using NextPost.Core.Models;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Interfaces
{
    public interface IJwtTokenService
    {
        public Task<string> CreateAccessTokenAsync(AppUser user);
        Task<RefreshToken?> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        RefreshToken CreateRefreshToken();
    }
}
