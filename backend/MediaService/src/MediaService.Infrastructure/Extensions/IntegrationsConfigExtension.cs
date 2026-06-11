using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediaService.Application.Interfaces;
using MediaService.Infrastructure.Integrations;
using Polly;
namespace MediaService.Infrastructure.Extensions
{
    public static class IntegrationsConfigExtension
    {
        public static IServiceCollection AddIntegrations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var authAddress = configuration.GetValue<string>("Destinations:AuthAddress");
            var postAddress = configuration.GetValue<string>("Destinations:PostAddress");
            var certificateAddress = configuration.GetValue<string>("Destinations:CertificateAddress");
            if (string.IsNullOrEmpty(authAddress) || string.IsNullOrEmpty(postAddress)
            || string.IsNullOrEmpty(certificateAddress)
            )
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
            services.AddHttpClient<IPostServicesClient, PostServicesClient>(client =>
            {
                client.BaseAddress = new Uri(postAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            services.AddHttpClient<IToolServicesClient, ToolServicesClient>(client =>
            {
                client.BaseAddress = new Uri(postAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            services.AddHttpClient<ICertificateServicesClient, CertificateServicesClient>(client =>
            {
                client.BaseAddress = new Uri(certificateAddress);
                client.Timeout = TimeSpan.FromSeconds(3);
            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromMilliseconds(
                        200 * retryAttempt)));
            return services;
        }
    }
}