using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Core.Models.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Bio { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
