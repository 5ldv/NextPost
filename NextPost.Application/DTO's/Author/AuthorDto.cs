using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Dtos
{
    public class AuthorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Bio { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public required UserDto User { get; set; }
    }
}
