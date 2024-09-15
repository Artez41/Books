using Books.Application.Logging;
using Books.Application.Repositories;
using System.Diagnostics;

namespace Books.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ILoggerAdapter<RatingService> _logger;

        public RatingService(IRatingRepository ratingRepository, ILoggerAdapter<RatingService> logger)
        {
            _ratingRepository = ratingRepository;
            _logger = logger;
        }

        public async Task<bool> RateBookAsync(Guid bookId, Guid userId, int rating, CancellationToken token = default)
        {
            _logger.LogInformation("User with id {0} rated book with id {1} on {2} mark", userId, bookId, rating);
            var stopWatch = Stopwatch.StartNew();

            try
            {
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
                _logger.LogInformation("Book with id {0} rated on mark {1} in {2}ms by user with id {3}", bookId, rating, stopWatch.ElapsedMilliseconds,
                    userId);
            }
        }
    }
}
