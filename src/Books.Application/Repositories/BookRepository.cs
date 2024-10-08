﻿using Dapper;
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
                insert into books (id, slug, title, author, description, yearofrelease, numberofpages)
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

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                delete from genres where bookid = @id
                """, new { id }, cancellationToken: token));

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                delete from books where id = @id
                """, new { id }, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                select count(1) from books where id = @id
                """, new { id }, cancellationToken: token));
        }

        public async Task<IEnumerable<Book>> GetAllAsync(GetAllBooksOptions options, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            string orderClause = string.Empty;

            if (options.SortField is not null)
            {
                orderClause = $"""
                    , b.{options.SortField}
                    order by b.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}
                    """;
            }

            var result = await connection.QueryAsync(new CommandDefinition($"""
                select 
                    b.*,
                    string_agg(distinct g.name, ',') as genres,
                    round(avg(r.rating), 1) as rating,
                    ur.rating as userrating
                from books b
                    left join genres g on b.id = g.bookid
                    left join ratings r on b.id = r.bookid
                    left join ratings ur on b.id = ur.bookid and ur.userid = @userId
                where (@title is null or b.title like ('%' || @title || '%'))
                    and (@author is null or b.author like ('%' || @author || '%')) 
                group by id, userrating {orderClause}
                limit @pageSize
                offset @pageOffset
                """, new
                    {
                       userId = options.UserId,
                       title = options.Title,
                       author = options.Author,
                       pageSize = options.PageSize,
                       pageOffset = (options.Page - 1) * options.PageSize
                    }, 
                    cancellationToken: token));

            return result.Select(x => new Book
            {
                Id = x.id,
                Title = x.title,
                Author = x.author,
                Description = x.description,
                TotalRating = (float?)x.rating,
                UserRating = (int?)x.userrating,
                YearOfRelease = x.yearofrelease,
                NumberOfPages = x.numberofpages,
                Genres = Enumerable.ToList(x.genres.Split(','))
            });
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

        public async Task<int> GetCountAsync(GetAllBooksOptions options, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<int>(new CommandDefinition("""
                select count(id) 
                from books b
                where (@title is null or b.title like ('%' || @title || '%'))
                    and (@author is null or b.author like ('%' || @author || '%'))
            """, new
                {
                    title = options.Title,
                    author = options.Author
                }, 
                cancellationToken: token));
        }

        public async Task<bool> UpdateAsync(Book book, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                delete from genres where bookid = @id
                """, new { book.Id }, cancellationToken: token));

            foreach (var genre in book.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    insert into genres (bookid, name)
                    values (@BookId, @Name)
                    """, new { BookId = book.Id, Name = genre }, cancellationToken: token));
            }

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                update books 
                set slug = @Slug, title = @Title, author = @Author, description = @Description, yearofrelease = @YearOfRelease, numberofpages = @NumberOfPages
                where id = @Id
                """, book, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }
    }
}
