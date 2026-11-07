using CertificateService.Infrastructure.Messaging.Handlers.Interfaces;
using CertificateService.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CertificateService.Infrastructure.Messaging.Consumers
{
    public class PostProjectionConsumer : BackgroundService
    {
        private readonly IPostProjectionHandler postProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<PostProjectionConsumer> logger;
        private readonly IConfiguration configuration;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public PostProjectionConsumer(
            IConnectionFactory _factory,
            ILogger<PostProjectionConsumer> _logger,
            IPostProjectionHandler _postProjectionHandler,
            IConfiguration _configuration
        )
        {
            this.factory = _factory;
            this.logger = _logger;
            this.postProjectionHandler = _postProjectionHandler;
            this.configuration = _configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                var serviceName = this.configuration["ServiceName"] ?? throw new InvalidOperationException("Configuração 'ServiceName' não encontrada.");
                this.consumer = new RabbitMQConsumer(this.connection, serviceName);
                this.consumer.RegisterHandler("PostUpdated", async message => { await this.postProjectionHandler.HandlePostProjectionUpdated(message);});
                this.consumer.RegisterHandler("PostDeleted", async message => { await this.postProjectionHandler.HandlePostProjectionDeleted(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Projetos do serviço de Postagens do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
                throw;
            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (connection != null)
                await this.connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}