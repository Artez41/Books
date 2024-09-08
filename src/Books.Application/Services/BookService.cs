using Books.Application.Logging;
using Books.Application.Models;
using Books.Application.Repositories;
using System.Diagnostics;

namespace Books.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerAdapter<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILoggerAdapter<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(Book book, CancellationToken token = default)
        {
            _logger.LogInformation("Creating book with id {0} and title: {1}", book.Id, book.Title);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _bookRepository.CreateAsync(book, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while creating a book");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Book with id {0} created in {1}ms", book.Id, stopWatch.ElapsedMilliseconds); ;
            }
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Deleting book with id: {0}", id);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _bookRepository.DeleteByIdAsync(id, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while deleting book with id {0}", id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Book with id {0} deleted in {1}ms", id, stopWatch.ElapsedMilliseconds); ;
            }        
        }

        public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Retrieving all books");
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _bookRepository.GetAllAsync(token);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving all books");
                throw;
            }
            finally 
            { 
                stopWatch.Stop();
                _logger.LogInformation("All books retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Retrieving book with id: {0}", id);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _bookRepository.GetByIdAsync(id, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving book with id {0}", id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Book with id {0} retrieved in {1}ms", id, stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<Book?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            _logger.LogInformation("Retrieve book by slug: {0}", slug);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _bookRepository.GetBySlugAsync(slug, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving book with slug {0}", slug);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Book with slug {0} retrieved in {1}ms", slug, stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<Book?> UpdateAsync(Book book, CancellationToken token = default)
        {
            _logger.LogInformation("Update book with id: {0}", book.Id);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                bool isBookExists = await _bookRepository.ExistsByIdAsync(book.Id, token);

                if (!isBookExists)
                    return null;

                bool isUpdated = await _bookRepository.UpdateAsync(book, token);

                if (!isUpdated)
                    return null;

                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while updating book with id {0}", book.Id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Book with id {0} updated in {1}ms", book.Id, stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
