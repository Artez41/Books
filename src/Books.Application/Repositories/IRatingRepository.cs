namespace Books.Application.Repositories
{
    public interface IRatingRepository
    {
        Task<bool> RateBookAsync(Guid bookId, Guid userId, int rating, CancellationToken token = default);
    }
}
