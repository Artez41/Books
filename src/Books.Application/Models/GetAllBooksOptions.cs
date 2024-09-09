namespace Books.Application.Models
{
    public class GetAllBooksOptions
    {
        public string Title { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
