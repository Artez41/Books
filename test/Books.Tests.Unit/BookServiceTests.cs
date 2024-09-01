﻿using Books.Application.Models;
using Books.Application.Repositories;
using Books.Application.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Books.Application.Tests.Unit
{
    public class BookServiceTests
    {
        private readonly BookService _sut;
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();

        public BookServiceTests()
        {
            _sut = new BookService(_bookRepository);
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
    }
}
