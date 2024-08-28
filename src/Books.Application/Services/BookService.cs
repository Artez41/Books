using Books.Application.Models;

namespace Books.Application.Services
{
    public class BookService : IBookService
    {
        public Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
