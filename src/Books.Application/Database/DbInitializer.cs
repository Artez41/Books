using Dapper;

namespace Books.Application.Database
{
    public class DbInitializer
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbInitializer(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync("""
                create table if not exists books (
                id UUID primary key,
                slug TEXT not null,
                title TEXT not null,
                author TEXT not null,
                description TEXT not null,
                yearofrelease integer not null,
                numberofpages integer not null);
            """);

            await connection.ExecuteAsync("""
                create unique index concurrently if not exists books_slug_idx
                on books
                using btree(slug);
            """);

            await connection.ExecuteAsync("""
                create table if not exists genres (
                bookId UUID references books (Id),
                name TEXT not null);
            """);
        }
    }
}
