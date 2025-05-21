namespace NextPost.Application.Dtos
{
    public class UpdateAuthorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Bio { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Location { get; set; }
    }
}
