using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using System.Text.Json;
using PostService.Infrastructure.Messaging.Events;
using PostService.Domain.Enums;
namespace PostService.Infrastructure.Messaging.Handlers
{
    public class CommentProjectionHandler : ICommentProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CommentProjectionHandler> logger;
        public CommentProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<CommentProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleCommentAdded(string message)
        {
            var payload = JsonSerializer.Deserialize<CommentEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case CommentType.Post:
                    await AddPostCommentCount(payload);
                    break;
                case CommentType.Tool:
                    await AddToolCommentCount(payload);
                    break;
            }
        }
        public async Task HandleCommentRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<CommentEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case CommentType.Post:
                    await RemovePosCommentCount(payload);
                    break;
                case CommentType.Tool:
                    await RemoveToolCommentCount(payload);
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
        private async Task AddPostCommentCount(CommentEvent payload)
        {
            var postServices = await GetPostServices();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            post.AddCommentCount();
            await postServices.Update(post);
        }
        private async Task RemovePosCommentCount(CommentEvent payload)
        {
            var postServices = await GetPostServices();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            post.RemoveCommentCount();
            await postServices.Update(post);
        }
        private async Task AddToolCommentCount(CommentEvent payload)
        {
            var toolsServices = await GetToolServices();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            tool.AddCommentCount();
            await toolsServices.Update(tool);
        }
        private async Task RemoveToolCommentCount(CommentEvent payload)
        {
            var toolsServices = await GetToolServices();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            tool.RemoveCommentCount();
            await toolsServices.Update(tool);
        }
    }
}