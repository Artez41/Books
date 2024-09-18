using Books.Application.Models;
using Books.Contracts.Requests;
using Books.Contracts.Responses;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
                UserRating = book.UserRating,
                TotalRating = book.TotalRating,
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
                Author = request.Author,
                SortField = request.SortBy?.Trim('+', '-'),
                SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                    request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public static GetAllBooksOptions WithUserId(this GetAllBooksOptions options, Guid? userId)
        {
            options.UserId = userId;
            return options;
        }

        public static IEnumerable<BookRatingResponse> MapToResponse(this IEnumerable<BookRating> ratings)
        {
            return ratings.Select(x => new BookRatingResponse
            {
                Rating = x.Rating,
                Slug = x.Slug,
                BookId = x.BookId
            });
        }
    }
}
