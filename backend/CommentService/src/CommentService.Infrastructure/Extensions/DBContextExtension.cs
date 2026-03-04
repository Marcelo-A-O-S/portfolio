using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CommentService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CommentService.Infrastructure.Extensions
{
    public static class DBContextExtension
    {
        public static IServiceCollection AddDBContextExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("OracleConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ConnectionString não configurada.");
            }
            services.AddDbContext<DBContext>(options =>
            {
                options.UseOracle(connectionString);
            });
            return services;
        }
    }
}