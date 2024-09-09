namespace Books.Contracts.Requests
{
    public class PagedRequest
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
