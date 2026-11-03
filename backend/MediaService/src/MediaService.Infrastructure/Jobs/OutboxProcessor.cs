using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaService.Infrastructure.Jobs
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
                    var unitOfWork = scope.ServiceProvider
                        .GetRequiredService<IUnitOfWork>();
                    var outboxServices = scope.ServiceProvider
                        .GetRequiredService<IOutboxMessageServices>();
                    var rabbitMQProducer = scope.ServiceProvider
                        .GetRequiredService<IRabbitMQProducer>();
                    var messages = await outboxServices.GetPendingMessages();
                    foreach (var message in messages)
                    {
                        try
                        {
                            await ChangeMessageStatus(unitOfWork, outboxServices, message, message => message.MarkAsProcessing());
                            try
                            {
                                await rabbitMQProducer.PublishRaw(
                                    message.EventType,
                                    message.Payload);
                            }
                            catch(Exception publishException)
                            {
                                try
                                {
                                    await ChangeMessageStatus(unitOfWork, outboxServices, message, message => message.RegisterFailure(publishException.Message));
                                }catch(Exception persistenceException)
                                {
                                    logger.LogError(
                                        persistenceException,
                                        "Não foi possível registrar a falha da mensagem {MessageId}", message.Id);
                                }
                                logger.LogError(
                                    publishException,
                                    "Falha ao publicar a mensagem {MessageId}", message.Id);
                                continue;
                            }
                            await ChangeMessageStatus(unitOfWork, outboxServices, message, message => message.MarkAsProcessed());
                        }
                        catch (Exception exception)
                        {
                            logger.LogError(
                                exception,
                                "Erro ao processar a mensagem Outbox {MessageId}",
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
        }
        private async Task ChangeMessageStatus(
            IUnitOfWork unitOfWork,
            IOutboxMessageServices outboxServices,
            OutboxMessage message,
            Action<OutboxMessage> changeStatus
        )
        {
            if (message is null)
                throw new InvalidOperationException(
                    $"Mensagem Outbox não encontrada.");
            await unitOfWork.BeginAsync();
            try
            {
                changeStatus(message);
                await outboxServices.Update(message);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                await unitOfWork.ReloadAsync(message);
                throw;
            } 
        }
    }
}