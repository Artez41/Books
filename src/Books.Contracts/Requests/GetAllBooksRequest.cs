namespace Books.Contracts.Requests
{
    public class GetAllBooksRequest : PagedRequest
    {
        public required string? Title { get; init; }
    }
}
