using Books.Application.Models;

namespace Books.Application.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<Book?> GetBySlugAsync(string slug, CancellationToken token = default);
        Task<bool> CreateAsync(Book book, CancellationToken token = default);
        Task<bool> UpdateAsync(Book book, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Book>> GetAllAsync(CancellationToken token = default);
    }
}
