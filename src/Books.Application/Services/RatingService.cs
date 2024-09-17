using Books.Application.Logging;
using Books.Application.Models;
using Books.Application.Repositories;
using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics;

namespace Books.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerAdapter<RatingService> _logger;

        public RatingService(IRatingRepository ratingRepository, ILoggerAdapter<RatingService> logger, IBookRepository bookRepository)
        {
            _ratingRepository = ratingRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<bool> DeleteRatingAsync(Guid bookId, Guid userId, CancellationToken token = default)
        {
            _logger.LogInformation("Delete rating of book with id {0} by user with id {1}", bookId, userId);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _ratingRepository.DeleteRatingAsync(bookId, userId, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while deleting rating");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Rating of user with id {0} for book with id {1} deleted in {2}ms", userId, bookId, 
                    stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<IEnumerable<BookRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        {
            _logger.LogInformation("Retrieving ratings of user with id {0}", userId);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                return await _ratingRepository.GetRatingsForUserAsync(userId, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving ratings");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Ratings of user with id {0} retrieved in {1}ms", userId, stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<bool> RateBookAsync(Guid bookId, Guid userId, int rating, CancellationToken token = default)
        {
            _logger.LogInformation("User with id {0} rated book with id {1} on {2} mark", userId, bookId, rating);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                if (rating is <= 0 or > 10)
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure
                        {
                            PropertyName = "Rating",
                            ErrorMessage = "Rating must be between 1 and 5"
                        }
                    });
                
                var bookExists = await _bookRepository.ExistsByIdAsync(bookId, token);
                if (!bookExists)
                    return false;

                return await _ratingRepository.RateBookAsync(bookId, userId, rating, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while rating book");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Book with id {0} rated on mark {1} in {2}ms by user with id {3}", bookId, rating, 
                    stopWatch.ElapsedMilliseconds, userId);
            }
        }
    }
}
