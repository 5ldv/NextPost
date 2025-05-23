namespace NextPost.Application.Exceptions
{
    public class InvalidCommentIdException : Exception
    {
        public InvalidCommentIdException(int commentId) : base($"Comment id ({commentId}) is not valid") { }
    }
}
