using Books.Application.Logging;
using Books.Application.Models;
using Books.Application.Repositories;
using Books.Application.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace Books.Application.Tests.Unit
{
    public class BookServiceTests
    {
        private readonly BookService _sut;
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
        private readonly IValidator<Book> _validator;
        private readonly IValidator<GetAllBooksOptions> _booksOptionsValidator;
        private readonly ILoggerAdapter<BookService> _logger = Substitute.For<ILoggerAdapter<BookService>>();
        private readonly GetAllBooksOptions _options;

        public BookServiceTests()
        {
            _options = new GetAllBooksOptions
            {
                Page = 1,
                PageSize = 10,
            };

            _sut = new BookService(_bookRepository, _logger, _validator, _booksOptionsValidator);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.GetByIdAsync(book.Id).Returns(book);

            // Act
            var result = await _sut.GetByIdAsync(book.Id);

            // Assert
            result.Should().BeEquivalentTo(book);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenBookNotExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepository.GetByIdAsync(bookId).ReturnsNull();

            // Act
            var result = await _sut.GetByIdAsync(bookId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.GetByIdAsync(book.Id).Returns(book);

            // Act
            await _sut.GetByIdAsync(book.Id);

            // Assert
            _logger.Received(1).LogInformation(Arg.Is("Retrieving book with id: {0}"), book.Id);
            _logger.Received(1).LogInformation(Arg.Is("Book with id {0} retrieved in {1}ms"), book.Id, Arg.Any<long>());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var sqliteException = new SqliteException("Something went wrong", 500);
            _bookRepository.GetByIdAsync(bookId).Throws(sqliteException);

            // Act
            var requestAction = async () => await _sut.GetByIdAsync(bookId);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");

            _logger.Received(1).LogInformation(Arg.Is("Retrieving book with id: {0}"), bookId);
            _logger.Received(1).LogError(sqliteException, Arg.Is("Something went wrong while retrieving book with id {0}"), bookId);
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.GetBySlugAsync(book.Slug).Returns(book);

            // Act
            var result = await _sut.GetBySlugAsync(book.Slug);

            // Assert
            result.Should().BeEquivalentTo(book);
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldReturnNull_WhenBookNotExist()
        {
            // Arrange
            _bookRepository.GetBySlugAsync(Arg.Any<string>()).ReturnsNull();

            // Act
            var result = await _sut.GetBySlugAsync(Arg.Any<string>());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.GetBySlugAsync(book.Slug).Returns(book);

            // Act
            await _sut.GetBySlugAsync(book.Slug);

            // Assert
            _logger.Received(1).LogInformation(Arg.Is("Retrieve book by slug: {0}"), Arg.Any<string>());
            _logger.Received(1).LogInformation(Arg.Is("Book with slug {0} retrieved in {1}ms"), Arg.Any<string>(), 
                Arg.Any<long>());
        }

        [Fact]
        public async Task GetBySlugAsync_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            var sqliteException = new SqliteException("Something went wrong", 500);
            _bookRepository.GetBySlugAsync(book.Slug).Throws(sqliteException);

            // Act
            var requestAction = async () => await _sut.GetBySlugAsync(book.Slug);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");
            _logger.Received(1).LogInformation(Arg.Is("Retrieve book by slug: {0}"), book.Slug);
            _logger.Received(1).LogError(sqliteException, Arg.Is("Something went wrong while retrieving book with slug {0}"), book.Slug);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnTrue_WhenBookCreated()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.CreateAsync(book).Returns(true);

            // Act
            var result = await _sut.CreateAsync(book);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFalse_WhenBookNotCreated()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.CreateAsync(book).Returns(false);

            // Act
            var result = await _sut.CreateAsync(book);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            _bookRepository.CreateAsync(book).Returns(true);

            // Act
            await _sut.CreateAsync(book);

            // Assert
            _logger.Received(1).LogInformation(Arg.Is("Creating book with id {0} and title: {1}"), book.Id, book.Title);
            _logger.Received(1).LogInformation(Arg.Is("Book with id {0} created in {1}ms"), book.Id, Arg.Any<long>());
        }

        [Fact]
        public async Task CreateAsync_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            var sqliteException = new SqliteException("Something went wrong", 500);
            _bookRepository.CreateAsync(book).ThrowsAsync(sqliteException);

            // Act
            var requestAction = async () => await _sut.CreateAsync(book);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");

            _logger.Received(1).LogInformation(Arg.Is("Creating book with id {0} and title: {1}"), book.Id, book.Title);
            _logger.Received(1).LogError(sqliteException, Arg.Is("Something went wrong while creating a book"));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Lord of the rings",
                Author = "J.R.R Tolkien",
                Description = "Very interesting book",
                YearOfRelease = 1923,
                NumberOfPages = 782,
                Genres = ["adventure"]
            };

            var books = new[]
            {
                book
            };

            _bookRepository.GetAllAsync(_options).Returns(books);

            // Act
            var result = await _sut.GetAllAsync(_options);

            // Assert
            result.Single().Should().BeEquivalentTo(book);
            result.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoBooksExist()
        {
            // Arrange
            _bookRepository.GetAllAsync(_options).Returns(Enumerable.Empty<Book>());

            // Act
            var result = await _sut.GetAllAsync(_options);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            _bookRepository.GetAllAsync(_options).Returns(Enumerable.Empty<Book>());

            // Act
            await _sut.GetAllAsync(_options);

            // Assert
            _logger.Received(1).LogInformation(Arg.Is("Retrieving all books"));
            _logger.Received(1).LogInformation(Arg.Is("All books retrieved in {0}ms"), Arg.Any<long>());
        }

        [Fact]
        public async Task GetAllAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
        {
            // Arrange
            var sqliteException = new SqliteException("Something went wrong", 500);
            _bookRepository.GetAllAsync(_options).Throws(sqliteException);

            // Act
            var requestAction = async () => await _sut.GetAllAsync(_options);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");
            _logger.Received(1).LogError(Arg.Is(sqliteException), Arg.Is("Something went wrong while retrieving all books"));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenBookNotExist()
        {
            // Arrange
            var updatedBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                Description = "Phylosofy book",
                YearOfRelease = 1866,
                NumberOfPages = 527,
                Genres = ["adventure"]
            };

            _bookRepository.ExistsByIdAsync(updatedBook.Id).Returns(false);

            // Act
            var result = await _sut.UpdateAsync(updatedBook);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenBookNotUpdated()
        {
            // Arrange
            var updatedBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                Description = "Phylosofy book",
                YearOfRelease = 1866,
                NumberOfPages = 527,
                Genres = ["adventure"]
            };

            _bookRepository.ExistsByIdAsync(updatedBook.Id).Returns(true);
            _bookRepository.UpdateAsync(updatedBook).Returns(false);

            // Act
            var result = await _sut.UpdateAsync(updatedBook);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBook_WhenBookUpdated()
        {
            // Arrange
            var updatedBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                Description = "Phylosofy book",
                YearOfRelease = 1866,
                NumberOfPages = 527,
                Genres = ["adventure"]
            };

            _bookRepository.ExistsByIdAsync(updatedBook.Id).Returns(true);
            _bookRepository.UpdateAsync(updatedBook).Returns(true);

            // Act
            var result = await _sut.UpdateAsync(updatedBook);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedBook);
        }

        [Fact]
        public async Task UpdateAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var updatedBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                Description = "Phylosofy book",
                YearOfRelease = 1866,
                NumberOfPages = 527,
                Genres = ["adventure"]
            };

            _bookRepository.ExistsByIdAsync(updatedBook.Id).Returns(true);
            _bookRepository.UpdateAsync(updatedBook).Returns(true);

            // Act
            var result = await _sut.UpdateAsync(updatedBook);

            // Assert
            _logger.Received(1).LogInformation(Arg.Is("Update book with id: {0}"), updatedBook.Id);
            _logger.Received(1).LogInformation(Arg.Is("Book with id {0} updated in {1}ms"), updatedBook.Id, Arg.Any<long>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var updatedBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Crime and Punishment",
                Author = "Fyodor Dostoevsky",
                Description = "Phylosofy book",
                YearOfRelease = 1866,
                NumberOfPages = 527,
                Genres = ["adventure"]
            };

            var sqliteException = new SqliteException("Something went wrong", 500);
            _bookRepository.ExistsByIdAsync(updatedBook.Id).Returns(true);
            _bookRepository.UpdateAsync(updatedBook).ThrowsAsync(sqliteException);

            // Act
            var requestAction = async () => await _sut.UpdateAsync(updatedBook);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");

            _logger.Received(1).LogInformation(Arg.Is("Update book with id: {0}"), updatedBook.Id);
            _logger.Received(1).LogError(sqliteException, Arg.Is("Something went wrong while updating book with id {0}"), updatedBook.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepository.DeleteByIdAsync(bookId).Returns(true);

            // Act
            var result = await _sut.DeleteByIdAsync(bookId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldNotDeleteBook_WhenBookNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepository.DeleteByIdAsync(bookId).Returns(false);

            // Act
            var result = await _sut.DeleteByIdAsync(bookId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepository.DeleteByIdAsync(bookId).Returns(true);

            // Act
            var result = await _sut.DeleteByIdAsync(bookId);

            // Assert
            _logger.LogInformation(Arg.Is("Deleting book with id: {0}"), bookId);
            _logger.LogInformation(Arg.Is("Book with id {0} deleted in {1}ms"), bookId, Arg.Any<long>());
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var sqliteException = new SqliteException("Something went wrong", 500);
            _bookRepository.DeleteByIdAsync(bookId).ThrowsAsync(sqliteException);

            // Act
            var requestAction = async () => await _sut.DeleteByIdAsync(bookId);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");

            _logger.LogInformation(Arg.Is("Deleting book with id: {0}"), bookId);
            _logger.LogError(sqliteException, Arg.Is("Book with id {0} deleted in {1}ms"), bookId, Arg.Any<long>());
        }
    }
}
