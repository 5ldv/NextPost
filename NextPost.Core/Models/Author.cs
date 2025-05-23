using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Core.Models
{
    public class Author
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required AppUser User { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Bio { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Location { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();

    }
}
