using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AuthService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDBContextExtension(configuration);
            services.AddDependencyInjection();
            services.AddHostedExtension();
            return services;
        }
    }
}