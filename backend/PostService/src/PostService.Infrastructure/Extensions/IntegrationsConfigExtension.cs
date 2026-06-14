using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Interfaces;
using PostService.Infrastructure.Integrations;
using Polly;
namespace PostService.Infrastructure.Extensions
{
    public static class IntegrationsConfigExtension
    {
        public static IServiceCollection AddIntegrations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var authAddress = configuration.GetValue<string>("Destinations:AuthAddress");
            if (string.IsNullOrEmpty(authAddress))
            {
                throw new Exception("Endereços de integrações não configurados.");
            }
            services.AddHttpClient<IAuthServicesClient, AuthServicesClient>(client =>
            {
                client.BaseAddress = new Uri(authAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            services.AddHttpClient<IUserServicesClient, UserServicesClient>(client =>
            {
                client.BaseAddress = new Uri(authAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            return services;
        }
    }
}