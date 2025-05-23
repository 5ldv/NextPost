namespace NextPost.Application.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException(int postId) : base($"Post with id ({postId}) not found") { }
        public PostNotFoundException() : base($"Post not found") { }
    }
}
