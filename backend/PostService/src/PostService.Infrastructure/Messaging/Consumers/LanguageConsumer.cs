using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace PostService.Infrastructure.Messaging.Consumers
{
    public class LanguageConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<LanguageConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public LanguageConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            ILogger<LanguageConsumer> _logger
        )
        {
            this.factory = _factory;
            this.scopeFactory = _scopeFactory;
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
                this.logger.LogInformation("Consumer de idiomas do serviço de Certificações do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}