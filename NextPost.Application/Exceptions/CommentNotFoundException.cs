namespace NextPost.Application.Exceptions
{
    public class CommentNotFoundException : Exception
    {
        public CommentNotFoundException(int commentId) : base($"Comment with id ({commentId}) not found") { }
        public CommentNotFoundException() : base($"Comment not found") { }
    }
}
