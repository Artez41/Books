using Dapper;
using Books.Application.Database;
using Books.Application.Models;

namespace Books.Application.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public BookRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var book = await connection.QueryFirstOrDefaultAsync<Book>(
                new CommandDefinition("""
                    select *
                    from books
                    where id = @id
                """, new { id }, cancellationToken: token));

            if (book is null)
                return null;

            return book;
        }
    }
}
