using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using CommentService.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
namespace CommentService.Infrastructure.Messaging.Consumers
{
    public class CommentConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly ILogger<CommentConsumer> logger;
        private readonly IConfiguration configuration;
        private IConnection? connection;
        private RabbitMQConsumer? consumer;
        public CommentConsumer(
            IConnectionFactory _factory,
            ILogger<CommentConsumer> _logger,
            IConfiguration _configuration
        )
        {
            this.factory = _factory;
            this.logger = _logger;
            this.configuration = _configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                this.connection = await this.factory.CreateConnectionAsync();
                var serviceName = this.configuration["ServiceName"] ?? throw new InvalidOperationException("Configuração 'ServiceName' não encontrada.");
                this.consumer = new RabbitMQConsumer(this.connection, serviceName);
                await consumer.Start();
                this.logger.LogInformation("Consumer dos Comentários do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
                throw;
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