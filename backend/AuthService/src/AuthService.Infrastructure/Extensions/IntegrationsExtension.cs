using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
namespace AuthService.Infrastructure.Extensions
{
    public static class IntegrationsExtension
    {
        public static IServiceCollection AddIntegrations(
            this IServiceCollection services
        )
        {
            services.AddHttpClient<LinkedinValidationService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            services.AddScoped<GoogleValidationService>();
            services.AddScoped<GithubValidationService>();
            services.AddScoped<GitHubClient>(x =>
                {
                    return new GitHubClient(
                        new ProductHeaderValue(
                            "AuthPortfolio"
                        )
                    );
                });
            services.AddScoped<IProviderFactory, ProviderFactory>();
            return services;
        }
    }
}