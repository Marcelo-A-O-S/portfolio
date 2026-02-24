using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions
{
    public static class DBInitializeExtension
    {
        public static IServiceProvider ApplyMigrations(
            this IServiceProvider service
        )
        {
            using(var scope = service.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<DBContext>();
                try
                {
                    context.Database.Migrate();
                }catch(Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return service;
        }
    }
}