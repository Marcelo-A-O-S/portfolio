using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CommentService.Infrastructure.Workers;
using RabbitMQ.Client;
using CommentService.Infrastructure.Messaging.Handlers.Interfaces;

namespace CommentService.Infrastructure.Messaging.Consumers
{
    public class PostConsumer : BackgroundService
    {
        private readonly IPostEventHandler postEventHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<PostConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public PostConsumer(
            IConnectionFactory _factory,
            ILogger<PostConsumer> _logger,
            IPostEventHandler _postEventHandler
        )
        {
            this.factory = _factory;
            this.logger = _logger;
            this.postEventHandler = _postEventHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
                this.consumer.RegisterHandler("PostDeleted", async message =>{ await this.postEventHandler.RemoveCache(message);});
                await consumer.Start();
                this.logger.LogInformation("Consumer das Postagens do serviço dos Comentários do RabbitMQ iniciado e aguardando mensagens...");
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