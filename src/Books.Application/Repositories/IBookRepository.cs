using Books.Application.Models;

namespace Books.Application.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Book?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    }
}
