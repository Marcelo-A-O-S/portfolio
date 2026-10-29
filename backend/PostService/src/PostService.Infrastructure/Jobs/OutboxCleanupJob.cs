using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
namespace PostService.Infrastructure.Jobs
{
    public class OutboxCleanupJob : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<OutboxCleanupJob> logger;
        public OutboxCleanupJob(
            IServiceScopeFactory _scopeFactory,
            ILogger<OutboxCleanupJob> _logger)
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
                    var cutoffDate = DateTime.UtcNow.AddDays(-7);
                    await outboxServices.DeleteProcessedBefore(cutoffDate);
                }
                catch(Exception exception)
                {
                    logger.LogError(exception, "Erro ao limpar a Outbox.");
                }
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}