namespace Books.Contracts.Responses
{
    public class BookResponse
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required string Description { get; init; }
        public required int NumberOfPages { get; init; }
        public required int YearOfRelease { get; init; }
        public float? TotalRating { get; init; }
        public int? UserRating { get; init; }
        public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
    }
}
