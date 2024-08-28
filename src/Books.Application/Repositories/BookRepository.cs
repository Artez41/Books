using Books.Application.Models;

namespace Books.Application.Repositories
{
    public class BookRepository : IBookRepository
    {
        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
