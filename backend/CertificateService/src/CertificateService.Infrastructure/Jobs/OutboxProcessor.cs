using CertificateService.Application.Interfaces;
using CertificateService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CertificateService.Infrastructure.Jobs
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<OutboxProcessor> logger;
        public OutboxProcessor(
            IServiceScopeFactory _scopeFactory,
            ILogger<OutboxProcessor> _logger)
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var outboxServices = scope.ServiceProvider
                        .GetRequiredService<IOutboxMessageServices>();
                    var rabbitMQProducer = scope.ServiceProvider
                        .GetRequiredService<IRabbitMQProducer>();
                    var messages = await outboxServices.GetPendingMessages();
                    foreach (var message in messages)
                    {
                        try
                        {
                            await rabbitMQProducer.Publish(
                                message.EventType,
                                message.Payload);
                            message.MarkAsProcessed();
                            await outboxServices.Update(message);
                        }
                        catch (Exception exception)
                        {
                            message.RegisterFailure(exception.Message);
                            await outboxServices.Update(message);
                            logger.LogError(
                                exception,
                                "Falha ao publicar a mensagem Outbox {MessageId}",
                                message.Id);
                        }
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(
                        exception,
                        "Erro ao processar a Outbox.");
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            throw new NotImplementedException();
        }
    }
}