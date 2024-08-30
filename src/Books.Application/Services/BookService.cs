using Books.Application.Models;
using Books.Application.Repositories;

namespace Books.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _bookRepository.GetByIdAsync(id, token);
        }
    }
}
