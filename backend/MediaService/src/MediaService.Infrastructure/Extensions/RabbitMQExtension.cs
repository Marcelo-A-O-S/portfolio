using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
namespace MediaService.Infrastructure.Extensions
{
    public static class RabbitMQExtension
    {
        public static IServiceCollection AddRabbitMQConnection(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var host = configuration.GetValue<string>("RabbitMQ:Host");
            var username = configuration.GetValue<string>("RabbitMQ:UserName");
            var password = configuration.GetValue<string>("RabbitMQ:Password");
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Chaves de configuração do RabbitMQ não registradas.");
            }
            services.AddSingleton<IConnectionFactory>(_ =>
            {
                return new ConnectionFactory()
                {
                    HostName = host,
                    UserName = username,
                    Password = password,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };
            });
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IConnectionFactory>();
                return factory.CreateConnectionAsync()
                        .GetAwaiter()
                        .GetResult();
            });
            return services;
        }
    }
}