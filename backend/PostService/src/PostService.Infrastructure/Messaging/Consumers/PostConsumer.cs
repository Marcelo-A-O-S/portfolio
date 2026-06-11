using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using PostService.Infrastructure.Workers;
using RabbitMQ.Client;
namespace PostService.Infrastructure.Messaging.Consumers
{
    public class PostConsumer : BackgroundService
    {
        private readonly ILikeProjectionHandler likeProjectionHandler;
        private readonly ICommentProjectionHandler commentProjectionHandler;
        private readonly IConnectionFactory factory;
        private readonly ILogger<PostConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public PostConsumer(
            IConnectionFactory _factory,
            ILogger<PostConsumer> _logger,
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
                this.consumer.RegisterHandler("PostLiked", async message => { await this.likeProjectionHandler.HandleLikeAdded(message); });
                this.consumer.RegisterHandler("PostUnliked", async message => { await this.likeProjectionHandler.HandleLikeRemoved(message); });
                this.consumer.RegisterHandler("PostCommentCreated", async message => { await this.commentProjectionHandler.HandleCommentAdded(message); });
                this.consumer.RegisterHandler("PostCommentDeleted", async message => { await this.commentProjectionHandler.HandleCommentRemoved(message); });
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Projetos do serviço de Postagens do RabbitMQ iniciado e aguardando mensagens...");
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
        
    }
}