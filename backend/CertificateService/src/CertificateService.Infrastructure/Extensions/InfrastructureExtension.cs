using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CertificateService.Infrastructure.Context;
namespace CertificateService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtensions(
            this IServiceCollection services, IConfiguration configuration
        ){
            services.AddDBContextExtension(configuration);
            services.AddDependencyInjectionExtensions();
            return services;
        }
    }
}