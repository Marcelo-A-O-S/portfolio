using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
namespace PostService.Infrastructure.Jobs
{
    public class OutboxRecovery : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<OutboxRecovery> logger;
        public OutboxRecovery(
            IServiceScopeFactory _scopeFactory,
            ILogger<OutboxRecovery> _logger)
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
                    var date = DateTime.UtcNow.AddMinutes(-5);
                    var messages = await outboxServices.GetProcessingMessagesByTimeLimit(date);
                    foreach (var message in messages)
                    {
                        await unitOfWork.BeginAsync();
                        try
                        {
                            message.RecoverProcessing();
                            await outboxServices.Update(message);
                            await unitOfWork.CommitAsync();
                        }
                        catch (Exception persistenceException)
                        {
                            logger.LogError(
                                persistenceException,
                                "Não foi possivel recuperar mensagem em processamento. {MessageId}", message.Id);
                            await unitOfWork.RollbackAsync();
                            await unitOfWork.ReloadAsync(message);
                        }
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(
                        exception,
                        "Erro ao processar a Outbox Recovery.");
                }
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}