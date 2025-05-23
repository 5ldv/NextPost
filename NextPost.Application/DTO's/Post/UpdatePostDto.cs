namespace NextPost.Application.DTO_s
{
    public class UpdatePostDto
    {
        public int postId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }

}
