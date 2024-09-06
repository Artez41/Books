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

        public Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            return _bookRepository.DeleteAsync(id, token);
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

        public async Task<Book?> UpdateAsync(Book book, CancellationToken token = default)
        {
            bool isBookExists = await _bookRepository.ExistsByIdAsync(book.Id, token);

            if (!isBookExists)
                return null;

            bool isUpdated = await _bookRepository.UpdateAsync(book, token);

            if (!isUpdated) 
                return null;

            return book;
        }
    }
}
