using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int AuthorId { get; set; }
        public required Author Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }

}
