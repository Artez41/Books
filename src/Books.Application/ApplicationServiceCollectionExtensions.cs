using Books.Application.Database;
using Books.Application.Logging;
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
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
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
