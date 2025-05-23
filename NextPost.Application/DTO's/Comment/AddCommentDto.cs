namespace NextPost.Application.DTO_s
{
    public class AddCommentDto
    {
        public int postId { get; set; }
        public required string Content { get; set; }
    }
}
