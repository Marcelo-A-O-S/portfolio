using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediaService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace MediaService.Infrastructure.Extensions
{
    public static class DBContextExtension
    {
        public static IServiceCollection AddDBContextExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ConnectionString não configurada.");
            }
            services.AddDbContext<DBContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }
    }
}