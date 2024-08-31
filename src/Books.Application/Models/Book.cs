namespace Books.Application.Models
{
    public class Book
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Slug { get; init; }
        public required string Author { get; init; }
        public required string Description { get; init; }
        public required int YearOfRelease { get; init; }
        public required int NumberOfPages { get; init; }
        public required List<string> Genres { get; init; } = new();
    }
}
