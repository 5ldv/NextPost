namespace NextPost.Application.DTO_s
{
    public class UpdateCommentDto
    {
        public required int CommentId { get; set; }
        public required string Content { get; set; }
    }
}
