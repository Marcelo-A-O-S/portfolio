using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using PostService.Domain.Enums;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace PostService.Infrastructure.Messaging.Handlers
{
    public class LikeProjectionHandler : ILikeProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<LikeProjectionHandler> logger;
        public LikeProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<LikeProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleLikeAdded(string message)
        {
            var payload = JsonSerializer.Deserialize<LikedEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case LikeType.Post:
                    await AddPostLikeCount(payload);
                    break;
                case LikeType.Tool:
                    await AddToolLikeCount(payload);
                    break;
            }
        }
        public async Task HandleLikeRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<LikedEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case LikeType.Post:
                    await RemovePostLikeCount(payload);
                    break;
                case LikeType.Tool:
                    await RemoveToolLikeCount(payload);
                    break;
            }
        }
        private async Task<IPostServices> GetPostServices()
        {
            using var scope = this.scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IPostServices>();
        }
        private async Task<IToolsServices> GetToolServices()
        {
            using var scope = this.scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IToolsServices>();
        }
        private async Task AddPostLikeCount(LikedEvent payload)
        {
            var postServices = await GetPostServices();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            post.AddLike();
            await postServices.Update(post);
        }
        private async Task RemovePostLikeCount(LikedEvent payload)
        {
            var postServices = await GetPostServices();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            post.RemoveLike();
            await postServices.Update(post);
        }
        private async Task AddToolLikeCount(LikedEvent payload)
        {
            var toolsServices = await GetToolServices();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            tool.AddLike();
            await toolsServices.Update(tool);
        }
        private async Task RemoveToolLikeCount(LikedEvent payload)
        {
            var toolsServices = await GetToolServices();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            tool.RemoveLike();
            await toolsServices.Update(tool);
        }
    }
}