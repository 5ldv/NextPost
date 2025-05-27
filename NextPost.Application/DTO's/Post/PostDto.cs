using NextPost.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.DTO_s
{
    public class PostDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required PostAuthorDto Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PostCommentDto> Comments { get; set; } = new();
    }
}
