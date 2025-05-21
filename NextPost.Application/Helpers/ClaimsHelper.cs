using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Helpers
{
    internal static class ClaimsHelper
    {

        public static async Task<List<Claim>> GetClaimsListAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var userRolesAsClaims = userRoles.Select(ur => new Claim(ClaimTypes.Role, ur));

            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
                };

            claims.AddRange(userClaims);
            claims.AddRange(userRolesAsClaims);

            return claims;
        }
    }
}
