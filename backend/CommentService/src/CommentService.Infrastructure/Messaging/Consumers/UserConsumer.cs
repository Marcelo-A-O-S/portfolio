using CommentService.Infrastructure.Messaging.Handlers.Interfaces;
using CommentService.Infrastructure.Workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
namespace CommentService.Infrastructure.Messaging.Consumers
{
    public class UserConsumer : BackgroundService
    {
        private readonly IUserEventHandler userEventHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<UserConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public UserConsumer(
            IConnectionFactory _factory,
            ILogger<UserConsumer> _logger,
            IUserEventHandler _userEventHandler
        )
        {
            this.factory = _factory;
            this.logger = _logger;
            this.userEventHandler = _userEventHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
                this.consumer.RegisterHandler("UserDeleted", async message => { await this.userEventHandler.RemoveCache(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Usuários do serviço dos Comentários do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
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
    }
}