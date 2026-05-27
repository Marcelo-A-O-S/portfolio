using AuthService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace AuthService.Infrastructure.Providers
{
    public class ProviderFactory : IProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        public ProviderFactory(
            IServiceProvider serviceProvider
        )
        {
            this.serviceProvider = serviceProvider;
        }
        public IProviderValidationService Get(string provider)
        {
            return provider.Trim().ToLowerInvariant() switch
            {
                "google" =>
                    serviceProvider.GetRequiredService<GoogleValidationService>(),
                "github" =>
                    serviceProvider.GetRequiredService<GithubValidationService>(),
                "linkedin" =>
                    serviceProvider.GetRequiredService<LinkedinValidationService>(),
                _ => throw new NotSupportedException(
                    "Provider não suportado"
                )
            };
        }
    }
}