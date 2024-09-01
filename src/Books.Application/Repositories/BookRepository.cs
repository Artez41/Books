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

        public async Task<bool> CreateAsync(Book book, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert int books (id, slug, title, author, description, yearofrelease, numberofpages)
                values (@Id, @Slug, @Title, @Author, @Description, @YearOfRelease, @NumberOfPages)
                """, book, cancellationToken: token));

            if (result > 0)
            {
                foreach (var genre in book.Genres)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        insert into genres (bookId, name)
                        values (@BookId, @Name)
                    """, new { BookId = book.Id, Name = genre }, cancellationToken: token));
                }
            }

            transaction.Commit();

            return result > 0;
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

        public async Task<Book?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var book = await connection.QueryFirstOrDefaultAsync<Book>(
                new CommandDefinition("""
                    select *
                    from books
                    where slug = @slug
                """, new { slug }, cancellationToken: token));

            if (book is null) 
                return null;

            return book;
        }
    }
}
