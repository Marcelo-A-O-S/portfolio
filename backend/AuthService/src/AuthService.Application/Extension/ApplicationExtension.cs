using Microsoft.Extensions.DependencyInjection;
namespace AuthService.Application.Extension
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtension(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjection();
            return services;
        }
    }
}