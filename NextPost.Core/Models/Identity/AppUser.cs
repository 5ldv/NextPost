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
        public Author? Author { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
