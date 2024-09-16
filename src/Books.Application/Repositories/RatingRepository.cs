using Books.Application.Database;
using Books.Application.Models;
using Dapper;

namespace Books.Application.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public RatingRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<BookRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.QueryAsync<BookRating>(new CommandDefinition("""
                    select r.rating, r.bookid, b.slug
                    from ratings r
                        inner join books b on r.bookid = b.id
                    where userid = @userId
                """, new { userId }, cancellationToken: token));
        }

        public async Task<bool> RateBookAsync(Guid bookId, Guid userId, int rating, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.ExecuteAsync(new CommandDefinition("""
                    insert into ratings(userid, bookid, rating)
                    values (@userId, @bookId, @rating)
                    on conflict (userid, bookid) do update
                        set rating = @rating
                """, new { userId, bookId, rating }, cancellationToken: token));

            return result > 0;
        }
    }
}
