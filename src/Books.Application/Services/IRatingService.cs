namespace Books.Application.Services
{
    public interface IRatingService
    {
        Task<bool> RateBookAsync(Guid bookId, Guid userId, int rating, CancellationToken token = default);
    }
}
