using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<User>, Generics<User>>();
            services.AddScoped<IGenerics<SocialAccount>,Generics<SocialAccount>>();
            services.AddScoped<IGenerics<RefreshToken>, Generics<RefreshToken>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISocialAccountRepository, SocialAccountRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}