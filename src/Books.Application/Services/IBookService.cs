using Books.Application.Models;

namespace Books.Application.Services
{
    public interface IBookService
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default);
    }
}
