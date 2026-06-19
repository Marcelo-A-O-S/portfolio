using Microsoft.Extensions.Hosting;
using MediaService.Infrastructure.Messaging.Handlers.Interfaces;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using MediaService.Infrastructure.Workers;

namespace MediaService.Infrastructure.Messaging.Consumers
{
    public class ToolConsumer : BackgroundService
    {
        private readonly IMediaFileProjectionHandler mediaFileProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<ToolConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public ToolConsumer(
            IConnectionFactory _factory,
            ILogger<ToolConsumer> _logger,
            IMediaFileProjectionHandler _mediaFileProjectionHandler
        )
        {
            this.factory = _factory;
            this.logger = _logger;
            this.mediaFileProjectionHandler = _mediaFileProjectionHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
                this.consumer.RegisterHandler("ToolMediaAttached", async message => { await this.mediaFileProjectionHandler.HandleMediaCommit(message); });
                this.consumer.RegisterHandler("ToolMediaDeleted", async message => { await this.mediaFileProjectionHandler.HandleMediaDelete(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer das Ferramentas do serviço de Midia do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}