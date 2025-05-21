using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Helpers
{
    internal static class UserContextHelper
    {
        public static int GetUserIdFromToken(IHttpContextAccessor _httpContextAccessor)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if(user == null || !(user.Identity?.IsAuthenticated ?? false))
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if(userIdClaim == null)
                throw new UnauthorizedAccessException("User ID claim not found in token.");

            return int.Parse(userIdClaim.Value);
        }

    }
}
