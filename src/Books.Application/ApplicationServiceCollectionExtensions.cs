using Books.Application.Database;
using Books.Application.Repositories;
using Books.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IBookService, BookService>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString) 
        {
            services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
            services.AddSingleton<DbInitializer>();
            return services;
        }
    }
}
