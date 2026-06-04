using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CertificateService.Application.Interfaces;
using CertificateService.Infrastructure.Integrations;
using Polly;
namespace CertificateService.Infrastructure.Extensions
{
    public static class IntegrationsConfigExtension
    {
        public static IServiceCollection AddIntegrations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var authAddress = configuration.GetSection("Destinations:AuthAddress")?.Value;
            var postAddress = configuration.GetSection("Destinations:PostAddress")?.Value;
            if (string.IsNullOrEmpty(authAddress) || string.IsNullOrEmpty(postAddress))
            {
                throw new Exception("Endereços de integrações não configurados.");
            }
            services.AddHttpClient<IInternalAuthClient, InternalAuthClient>(client =>
            {
                client.BaseAddress = new Uri(authAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            services.AddHttpClient<IPostServicesClient, PostServicesClient>(client =>
            {
                client.BaseAddress = new Uri(postAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            return services;
        }
    }
}