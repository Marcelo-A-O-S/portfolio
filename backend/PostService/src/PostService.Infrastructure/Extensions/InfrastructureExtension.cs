using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace PostService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDBContextExtension(configuration);
            services.AddDependencyInjection();
            return services;
        }
    }
}