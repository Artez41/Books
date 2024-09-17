namespace Books.Application.Models
{
    public class BookRating
    {
        public required Guid BookId { get; init; }
        public required string Slug { get; init; }
        public required int Rating { get; init; }
    }
}
