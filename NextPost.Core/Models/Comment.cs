namespace NextPost.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public required Post Post { get; set; }
        public int AuthorId { get; set; }
        public required Author Author { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

}
