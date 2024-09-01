using Books.Application.Models;
using Books.Contracts.Requests;

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
    }
}
