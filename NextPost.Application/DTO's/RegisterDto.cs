using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Dtos
{
    public class RegisterDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; } 
        public required string Username { get; set; } 
        public required string Email { get; set; } 
        public required string Password { get; set; } 
        public required string ConfirmPassword { get; set; }
    }
}
