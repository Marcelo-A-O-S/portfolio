using CommentService.Infrastructure.Messaging.Events;
using CommentService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CommentService.Application.Interfaces;
namespace CommentService.Infrastructure.Messaging.Handlers
{
    public class PostEventHandler : IPostEventHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<PostEventHandler> logger;
        public PostEventHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<PostEventHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task RemoveCache(string message)
        {
            var payload = JsonSerializer.Deserialize<PostRemoveEvent>(message);
            if(payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IPostCacheServices>();
            await cache.RemovePostCache($"post:exists:{payload.PostId}");
        }
    }
}