using Books.Application.Models;

namespace Books.Application.Repositories
{
    internal interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
