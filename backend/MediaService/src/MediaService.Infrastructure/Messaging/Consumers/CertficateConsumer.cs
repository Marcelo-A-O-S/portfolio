using MediaService.Infrastructure.Messaging.Handlers.Interfaces;
using MediaService.Infrastructure.Workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
namespace MediaService.Infrastructure.Messaging.Consumers
{
    public class CertficateConsumer : BackgroundService
    {
        private readonly IMediaFileProjectionHandler mediaFileProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<CertficateConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public CertficateConsumer(
            IConnectionFactory _factory,
            ILogger<CertficateConsumer> _logger,
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
                this.consumer.RegisterHandler("CertificateMediaCommited", async message => { await this.mediaFileProjectionHandler.HandleMediaCommit(message); });
                this.consumer.RegisterHandler("CertificateMediaDeleted", async message => { await this.mediaFileProjectionHandler.HandleMediaDelete(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Certificados do serviço de Midia do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}