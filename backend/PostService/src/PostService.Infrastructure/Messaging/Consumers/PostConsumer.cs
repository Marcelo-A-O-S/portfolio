
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Workers;
using RabbitMQ.Client;
using System.Text.Json;

namespace PostService.Infrastructure.Messaging.Consumers
{
    public class PostConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ILogger<PostConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public PostConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            IRabbitMQProducer _rabbitMQProducer,
            ILogger<PostConsumer> _logger
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
                this.consumer.RegisterHandler("remove-user", async message => { await this.RemoveUserCache(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Projetos do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
        public override async Task StopAsync(
        CancellationToken cancellationToken)
        {
            if(connection != null)
                await this.connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
        private async Task RemoveUserCache(string message)
        {
            var payload = JsonSerializer.Deserialize<UserRemoveEvent>(message);
            if(payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IUserCacheServices>();
            await cache.RemoveUserCache(
                $"user:{payload.UserId}"
            );
        }
    }
}