using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Workers;
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGenerics<User>, Generics<User>>();
            services.AddScoped<IGenerics<SocialAccount>,Generics<SocialAccount>>();
            services.AddScoped<IGenerics<RefreshToken>, Generics<RefreshToken>>();
            services.AddScoped<IGenerics<OutboxMessage>, Generics<OutboxMessage>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISocialAccountRepository, SocialAccountRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            return services;
        }
    }
}