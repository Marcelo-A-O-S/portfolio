using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Infrastructure.Workers;
using RabbitMQ.Client;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;

namespace PostService.Infrastructure.Messaging.Consumers
{
    public class ToolConsumer : BackgroundService
    {
        private readonly ILikeProjectionHandler likeProjectionHandler;
        private readonly ICommentProjectionHandler commentProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<ToolConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public ToolConsumer(
            IConnectionFactory _factory,
            ILogger<ToolConsumer> _logger,
            ILikeProjectionHandler _likeProjectionHandler,
            ICommentProjectionHandler _commentProjectionHandler
        )
        {
            this.factory = _factory;
            this.logger = _logger;
            this.likeProjectionHandler = _likeProjectionHandler;
            this.commentProjectionHandler = _commentProjectionHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
                this.consumer.RegisterHandler("ToolLiked", async message => { await this.likeProjectionHandler.HandleLikeAdded(message); });
                this.consumer.RegisterHandler("ToolUnliked", async message => { await this.likeProjectionHandler.HandleLikeRemoved(message); });
                this.consumer.RegisterHandler("ToolCommentCreated", async message => { await this.commentProjectionHandler.HandleCommentAdded(message); });
                this.consumer.RegisterHandler("ToolCommentDeleted", async message => { await this.commentProjectionHandler.HandleCommentRemoved(message); });
                await consumer.Start();
                this.logger.LogInformation("Consumer das Ferramentas do serviço de Postagens do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}