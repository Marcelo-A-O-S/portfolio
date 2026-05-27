using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Workers;

namespace AuthService.Infrastructure.Messaging.Consumers
{
    public class UserConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ILogger<UserConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public UserConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            IRabbitMQProducer _rabbitMQProducer,
            ILogger<UserConsumer> _logger
        )
        {
            this.factory = _factory;
            this.scopeFactory = _scopeFactory;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.logger = _logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
             try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Usuários do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if(connection != null)
                await this.connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}