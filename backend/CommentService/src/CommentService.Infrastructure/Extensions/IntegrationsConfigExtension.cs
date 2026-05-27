using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CommentService.Infrastructure.Integrations;
using CommentService.Application.Interfaces;
using Polly;
namespace CommentService.Infrastructure.Extensions
{
    public static class IntegrationsConfigExtension
    {
        public static IServiceCollection AddIntegrations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var authAddress = configuration.GetSection("Destinations:AuthAddress")?.Value;
            if (string.IsNullOrEmpty(authAddress))
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