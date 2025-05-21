using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NextPost.Application.Dtos
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
