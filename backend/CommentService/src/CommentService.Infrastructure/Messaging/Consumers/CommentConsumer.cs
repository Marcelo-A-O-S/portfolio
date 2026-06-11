using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using CommentService.Infrastructure.Workers;
using System.Text.Json;
using CommentService.Infrastructure.Messaging.Events;
using CommentService.Application.Interfaces;

namespace CommentService.Infrastructure.Messaging.Consumers
{
    public class CommentConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CommentConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public CommentConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            ILogger<CommentConsumer> _logger
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
                this.consumer.RegisterHandler("UserDeleted", async message => { await this.RemoveUserCache(message);});
                this.consumer.RegisterHandler("PostDeleted", async message =>{ await this.RemovePostCache(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Comentários do RabbitMQ iniciado e aguardando mensagens...");
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
        private async Task RemoveUserCache(string message)
        {
            var payload = JsonSerializer.Deserialize<UserRemoveEvent>(message);
            if(payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IUserCacheServices>();
            await cache.RemoveUserCache($"user:exists:{payload.UserId}");
        }
        private async Task RemovePostCache(string message)
        {
            var payload = JsonSerializer.Deserialize<PostRemoveEvent>(message);
            if(payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IPostCacheServices>();
            await cache.RemovePostCache($"post:exists:{payload.PostId}");
        }
    }
}