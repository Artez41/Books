using Books.Application.Models;
using Books.Contracts.Requests;
using Books.Contracts.Responses;

namespace Books.Api.Mapping
{
    public static class ContractMapping
    {
        public static Book MapToBook(this CreateBookRequest request)
        {
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                Description = request.Description,
                YearOfRelease = request.YearOfRelease,
                NumberOfPages = request.NumberOfPages,
                Genres = request.Genres.ToList()
            };
        }

        public static Book MapToBook(this UpdateBookRequest request, Guid id)
        {
            return new Book
            {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                Description = request.Description,
                YearOfRelease = request.YearOfRelease,
                NumberOfPages = request.NumberOfPages,
                Genres = request.Genres.ToList()
            };
        }

        public static BookResponse MapToResponse(this Book book)
        {
            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                YearOfRelease = book.YearOfRelease,
                NumberOfPages = book.NumberOfPages,
                Genres = book.Genres.ToList()
            };
        }

        public static BooksResponse MapToResponse(this IEnumerable<Book> books, int page,
            int pageSize, int totalCount)
        {
            return new BooksResponse
            {
                Items = books.Select(MapToResponse),
                Page = page,
                PageSize = pageSize,
                Total = totalCount
            };
        }

        public static GetAllBooksOptions MapToOptions(this GetAllBooksRequest request)
        {
            return new GetAllBooksOptions
            {
                Title = request.Title,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
