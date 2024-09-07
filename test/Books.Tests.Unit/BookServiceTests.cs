using Books.Application.Models;
using Books.Application.Repositories;
using Books.Application.Services;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Books.Application.Tests.Unit
{
    public class BookServiceTests
    {
        private readonly BookService _sut;
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
        private readonly ILogger<BookService> _logger = Substitute.For<ILogger<BookService>>();

        public BookServiceTests()
        {
            _sut = new BookService(_bookRepository, _logger);
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
            _bookRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

            // Act
            var result = await _sut.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
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
        public async Task CreateAsync_ShouldCreateBook_WhenBookCreated()
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

            _bookRepository.GetAllAsync().Returns(books);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Single().Should().BeEquivalentTo(book);
            result.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoBooksExist()
        {
            // Arrange
            _bookRepository.GetAllAsync().Returns(Enumerable.Empty<Book>());

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
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
    }
}
