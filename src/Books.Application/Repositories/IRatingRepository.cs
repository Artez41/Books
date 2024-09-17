using Books.Application.Models;

namespace Books.Application.Repositories
{
    public interface IRatingRepository
    {
        Task<bool> RateBookAsync(Guid bookId, Guid userId, int rating, CancellationToken token = default);
        Task<IEnumerable<BookRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
        Task<bool> DeleteRatingAsync(Guid bookId, Guid userId, CancellationToken token = default);
    }
}
