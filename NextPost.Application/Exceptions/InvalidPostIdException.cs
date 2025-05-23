namespace NextPost.Application.Exceptions
{
    public class InvalidPostIdException : Exception
    {
        public InvalidPostIdException(int Post) : base($"Post id ({Post}) is not valid") { }
    }
}
