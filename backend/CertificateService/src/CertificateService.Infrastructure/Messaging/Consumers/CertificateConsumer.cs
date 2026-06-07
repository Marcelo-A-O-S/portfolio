using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using CertificateService.Infrastructure.Workers;
using System.Text.Json;
using CertificateService.Application.Interfaces;
using CertificateService.Infrastructure.Messaging.Events;
namespace CertificateService.Infrastructure.Messaging.Consumers
{
    public class CertificateConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CertificateConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public CertificateConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            ILogger<CertificateConsumer> _logger
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
                this.consumer.RegisterHandler("delete-post", async message =>{ await this.RemovePostCache(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Certificados do RabbitMQ iniciado e aguardando mensagens...");
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
        private async Task RemovePostCache(string message)
        {
            var payload = JsonSerializer.Deserialize<PostRemoveEvent>(message);
            if(payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IPostCacheServices>();
            await cache.RemovePostCache($"post:{payload.PostId}");
        }
    }
}