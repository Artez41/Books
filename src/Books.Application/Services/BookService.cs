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

        public Task<bool> CreateAsync(Book book, CancellationToken token = default)
        {
            return _bookRepository.CreateAsync(book, token);
        }

        public Task<IEnumerable<Book>> GetAllAsync(CancellationToken token = default)
        {
            return _bookRepository.GetAllAsync(token);
        }

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _bookRepository.GetByIdAsync(id, token);
        }

        public Task<Book?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            return _bookRepository.GetBySlugAsync(slug, token);
        }

        public Task<bool> UpdateAsync(Book book, CancellationToken token = default)
        {
            return _bookRepository.UpdateAsync(book, token);
        }
    }
}
