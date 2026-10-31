using System.Text.Json;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Infrastructure.Messaging.Events;
using CertificateService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace CertificateService.Infrastructure.Messaging.Handlers
{
    public class PostProjectionHandler : IPostProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<PostProjectionHandler> logger;
        public PostProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<PostProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandlePostProjectionDeleted(string message)
        {
            var payload = JsonSerializer.Deserialize<PostDeletedEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var postProjectionServices = scope.ServiceProvider.GetRequiredService<IPostProjectionServices>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var postProjection = await postProjectionServices.FindBy(pp => pp.PostId == payload.PostId);
            if (postProjection == null)
            {
                this.logger.LogWarning("Evento recebido para projeção de publicação inexistente. PostProjectionId: {PostId}", payload.PostId);
                return;
            }
            await unitOfWork.BeginAsync();
            try
            {
                await postProjectionServices.Delete(postProjection);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task HandlePostProjectionRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<PostRemovedEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var postProjectionServices = scope.ServiceProvider.GetRequiredService<IPostProjectionServices>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task HandlePostProjectionUpdated(string message)
        {
            var payload = JsonSerializer.Deserialize<PostUpdatedEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var postProjectionServices = scope.ServiceProvider.GetRequiredService<IPostProjectionServices>();
            var postContentProjectionServices = scope.ServiceProvider.GetRequiredService<IPostContentProjectionServices>();
            var languageProjectionServices = scope.ServiceProvider.GetRequiredService<ILanguageProjectionServices>();
            var mediaProjectionServices = scope.ServiceProvider.GetRequiredService<IMediaProjectionServices>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var postProjection = await postProjectionServices.FindWithContentBy(pp => pp.PostId == payload.PostId);
            if (postProjection == null)
            {
                this.logger.LogWarning("Evento recebido para projeção de publicação inexistente. PostId: {PostId}", payload.PostId);
                return;
            }

            foreach (var item in payload.PostContents)
            {
                if (item.Language?.Id is not Guid languageId)
                {
                    logger.LogWarning(
                        "Evento de projeção de publicação sem identificador de idioma válido. PostId: {PostId}",
                        payload.PostId);

                    return;
                }
                if (item.Id is not Guid postContentId)
                {
                    this.logger.LogWarning("Evento de projeção de publicação sem identificador de conteudo válido. PostId: {CertificateId}", payload.PostId);
                    return;
                }
                var postContentProjection = postProjection.PostContents.FirstOrDefault(pc => pc.PostContentId == postContentId);
                if (postContentProjection != null)
                {
                    postContentProjection.Update(item.Title, item.Description);
                    continue;
                }
                var languageProjection = await languageProjectionServices.FindBy(lp => lp.LanguageId == languageId);
                if (languageProjection == null)
                {
                    logger.LogWarning(
                        "Idioma {LanguageId} não encontrado para o certificado {CertificateId}",
                        languageId,
                        payload.PostId);

                    continue;
                }
                var newProjection = new PostContentProjection(
                    postProjection.Id,
                    postContentId,
                    languageProjection.Id,
                    item.Title,
                    item.Description
                );
                postProjection.AddPostContentProjection(newProjection);
            }

            var incomingContentIds = payload.PostContents
                .Select(item => new
                {
                    PostContentId = item.Id!.Value
                })
                .ToHashSet();
            var removedContents = postProjection.PostContents
                .Where(pcp => !incomingContentIds.Contains(new
                {
                    pcp.PostContentId
                }))
                .ToList();
            await unitOfWork.BeginAsync();
            try
            {
                if (postProjection.MediaProjection.MediaId != payload.Media.MediaId)
                {
                    var oldMediaProjection = await mediaProjectionServices.FindBy(mp => mp.MediaId == postProjection.MediaProjection.MediaId);
                    var newMediaProjection = new MediaProjection(
                            payload.Media.MediaId,
                            payload.Media.Url
                        );
                    newMediaProjection.GenerateId();
                    postProjection.SetThumbnail(newMediaProjection.Id);
                    await mediaProjectionServices.Save(newMediaProjection);
                    if (oldMediaProjection != null)
                    {
                        await mediaProjectionServices.Delete(oldMediaProjection);
                    }
                }
                foreach (var content in removedContents)
                {
                    postProjection.RemovePostContentProjection(content);
                    await postContentProjectionServices.Delete(content);
                }
                await postProjectionServices.Update(postProjection);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}