using CertificateService.Infrastructure.Messaging.Handlers.Interfaces;
using CertificateService.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
namespace CertificateService.Infrastructure.Messaging.Consumers
{
    public class LanguageConsumer : BackgroundService
    {
        private readonly ILanguageProjectionHandler languageProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<LanguageConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public LanguageConsumer(
            ILanguageProjectionHandler _languageProjectionHandler,
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            ILogger<LanguageConsumer> _logger
        )
        {
            this.factory = _factory;
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
            this.languageProjectionHandler = _languageProjectionHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
                this.consumer.RegisterHandler("LanguageDeleted", async message => { await this.languageProjectionHandler.HandleLanguageDeleted(message); });
                this.consumer.RegisterHandler("LanguageUpdated", async message => { await this.languageProjectionHandler.HandleLanguageUpdated(message); });
                await consumer.Start();
                this.logger.LogInformation("Consumer de Idiomas do serviço de Certificações do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}