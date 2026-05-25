using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Interfaces;
using PostService.Infrastructure.Cache;
using StackExchange.Redis;

namespace PostService.Infrastructure.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var connectionString = configuration.GetValue<string>("Redis:ConnectionString");
            if(string.IsNullOrEmpty(connectionString))
                throw new Exception("Chave de conexão do redis não configurada.");
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = false;
                options.ReconnectRetryPolicy = new ExponentialRetry(5000);
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddSingleton<ICacheService, RedisCacheService>();
            return services;
        }
    }
}