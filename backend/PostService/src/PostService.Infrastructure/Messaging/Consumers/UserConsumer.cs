using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text.Json;
using PostService.Application.Caching.User;
namespace PostService.Infrastructure.Messaging.Consumers
{
    public class UserConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<UserConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public UserConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            ILogger<UserConsumer> _logger
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
                this.consumer.RegisterHandler("UserDeleted", async message => { await this.RemoveUserCache(message); });
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Usuários do serviço de Postagens do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (connection != null)
                await this.connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
        private async Task RemoveUserCache(string message)
        {
            var payload = JsonSerializer.Deserialize<UserRemoveEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IUserCacheServices>();
            await cache.RemoveUserCache($"user:exists:{payload.UserId}");
        }
    }
}