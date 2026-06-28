using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using CommentService.Infrastructure.Workers;
namespace CommentService.Infrastructure.Messaging.Consumers
{
    public class CommentConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly ILogger<CommentConsumer> logger;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public CommentConsumer(
            IConnectionFactory _factory,
            ILogger<CommentConsumer> _logger
        )
        {
            this.factory = _factory;
            this.logger = _logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                this.consumer = new RabbitMQConsumer(this.connection);
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
    }
}