using Microsoft.Extensions.Hosting;

namespace AuthService.Infrastructure.Messaging.Consumers
{
    public class UserConsumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}