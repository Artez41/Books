namespace Books.Contracts.Responses
{
    public class BooksResponse
    {
        public required IEnumerable<BookResponse> Items { get; init; } = Enumerable.Empty<BookResponse>();
    }
}
