namespace Books.Contracts.Requests
{
    public class CreateBookRequest
    {
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required string Description { get; init; }
        public required int YearOfRelease { get; init; }
        public required int NumberOfPages { get; init; }
        public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
    }
}
