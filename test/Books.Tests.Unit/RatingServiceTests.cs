﻿using Books.Application.Logging;
using Books.Application.Models;
using Books.Application.Repositories;
using Books.Application.Services;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Sdk;

namespace Books.Application.Tests.Unit
{
    public class RatingServiceTests
    {
        private readonly RatingService _sut;
        private readonly IRatingRepository _ratingRepository = Substitute.For<IRatingRepository>();
        private readonly ILoggerAdapter<RatingService> _logger = Substitute.For<ILoggerAdapter<RatingService>>();

        public RatingServiceTests()
        {
            _sut = new RatingService(_ratingRepository, _logger);
        }

        [Fact]
        public async Task RateBookAsync_ShouldReturnTrue_WhenBookRated()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            int rating = 10;

            _ratingRepository.RateBookAsync(bookId, userId, rating).Returns(true);

            // Act
            var result = await _sut.RateBookAsync(bookId, userId, rating);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task RateBookAsync_ShouldReturnFalse_WhenBookNotRated()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            int rating = 1;

            _ratingRepository.RateBookAsync(bookId, userId, rating).Returns(false);

            // Act
            var result = await _sut.RateBookAsync(bookId, userId, rating);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task RateBookAsync_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            int rating = 7;

            _ratingRepository.RateBookAsync(bookId, userId, rating).Returns(true);

            // Act
            _ = await _sut.RateBookAsync(bookId, userId, rating);

            // Assert
            _logger.Received(1).LogInformation("User with id {0} rated book with id {1} on {2} mark", userId, 
                bookId, rating);
            _logger.Received(1).LogInformation("Book with id {0} rated on mark {1} in {2}ms by user with id {3}",
                bookId, rating, Arg.Any<long>(), userId);
        }

        [Fact]
        public async Task RateBookAsync_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var sqliteException = new SqliteException("Something went wrong", 500);
            var bookId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            int rating = 7;

            _ratingRepository.RateBookAsync(bookId, userId, rating).ThrowsAsync(sqliteException);

            // Act
            var requestAction = async () => await _sut.RateBookAsync(bookId, userId, rating);

            // Assert
            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");

            _logger.Received(1).LogInformation("User with id {0} rated book with id {1} on {2} mark", userId,
                bookId, rating);
            _logger.Received(1).LogError(sqliteException, "Something went wrong while rating book");
        }

        [Fact]
        public async Task GetUserRatings_ShouldReturnRatings_WhenRatingsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userRating = new BookRating
            {
                BookId = Guid.NewGuid(),
                Slug = "slug",
                Rating = 5
            };

            var userRatings = new[]
            {
                userRating
            };

            _ratingRepository.GetRatingsForUserAsync(userId).Returns(userRatings);

            // Act
            var result = await _sut.GetRatingsForUserAsync(userId);

            // Assert
            result.Single().Should().BeEquivalentTo(userRating);
            result.Should().BeEquivalentTo(userRatings);
        }

        [Fact]
        public async Task GetUserRatings_ShouldReturnEmptyList_WhenNoRatingsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _ratingRepository.GetRatingsForUserAsync(userId).Returns(Enumerable.Empty<BookRating>());

            // Act
            var result = await _sut.GetRatingsForUserAsync(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserRatings_ShouldLogMessages_WhenInvoked()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var userRating = new BookRating
            {
                BookId = Guid.NewGuid(),
                Slug = "slug",
                Rating = 5
            };

            var userRatings = new[]
            {
                userRating
            };

            _ratingRepository.GetRatingsForUserAsync(userId).Returns(userRatings);

            // Act
            _ = await _sut.GetRatingsForUserAsync(userId);

            // Assert
            _logger.Received(1).LogInformation(Arg.Is("Retrieving ratings of user with id {0}"), userId);
            _logger.Received(1).LogInformation(Arg.Is("Ratings of user with id {0} retrieved in {1}ms"), userId,
                Arg.Any<long>());
        }

        [Fact]
        public async Task GetUserRatings_ShouldLogMessages_WhenExceptionIsThrown()
        {
            // Arrange
            var sqliteException = new SqliteException("Something went wrong", 500);
            var userId = Guid.NewGuid();

            _ratingRepository.GetRatingsForUserAsync(userId).ThrowsAsync(sqliteException);

            // Act
            var requestAction = async () => await _sut.GetRatingsForUserAsync(userId);

            // Assert

            await requestAction.Should()
                .ThrowAsync<SqliteException>().WithMessage("Something went wrong");

            _logger.Received(1).LogInformation(Arg.Is("Retrieving ratings of user with id {0}"), userId);
            _logger.Received(1).LogError(sqliteException, Arg.Is("Something went wrong while retrieving ratings"));
        }
    }
}
