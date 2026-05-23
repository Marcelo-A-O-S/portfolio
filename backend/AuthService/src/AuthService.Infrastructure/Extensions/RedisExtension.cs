using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
namespace AuthService.Infrastructure.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var connectionString = configuration.GetSection("Redis:ConnectionString")?.Value;
            if(string.IsNullOrEmpty(connectionString))
                throw new Exception("Chave de conexão do redis não configurada.");
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = false;
                options.ReconnectRetryPolicy = new ExponentialRetry(5000);
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddSingleton<ICacheServices, RedisCacheService>();
            return services;
        }
    }
}