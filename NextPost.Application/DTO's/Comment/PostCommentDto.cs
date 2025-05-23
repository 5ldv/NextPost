namespace NextPost.Application.DTO_s
{
    public class PostCommentDto
    {
        public required CommentAuthorDto Author { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
