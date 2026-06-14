using Microsoft.Extensions.Hosting;
using MediaService.Infrastructure.Workers;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using MediaService.Infrastructure.Messaging.Handlers.Interfaces;
namespace MediaService.Infrastructure.Messaging.Consumers
{
    public class PostConsumer : BackgroundService
    {
        private readonly IMediaFileProjectionHandler mediaFileProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<PostConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public PostConsumer(
            IConnectionFactory _factory,
            ILogger<PostConsumer> _logger,
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
                this.consumer.RegisterHandler("PostMediaCommited", async message => { await this.mediaFileProjectionHandler.HandleMediaCommit(message);});
                this.consumer.RegisterHandler("PostMediaDeleted", async message => { await this.mediaFileProjectionHandler.HandleMediaDelete(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Projetos do serviço de Midia do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}