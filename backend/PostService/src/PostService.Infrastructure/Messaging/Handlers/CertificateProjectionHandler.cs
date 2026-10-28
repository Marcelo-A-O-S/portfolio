using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
namespace PostService.Infrastructure.Messaging.Handlers
{
    public class CertificateProjectionHandler : ICertificateProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CertificateProjectionHandler> logger;
        public CertificateProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<CertificateProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleCertificateAdded(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateAddedEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            var postServices = scope.ServiceProvider.GetRequiredService<IPostServices>();
            var languageServices = scope.ServiceProvider.GetRequiredService<ILanguageServices>();
            if (payload == null)
                return;
            var associationExists = await certificateProjectionServices.CheckAssociationWithPost(payload.CertificateId, payload.PostId);
            if (associationExists)
            {
                this.logger.LogWarning("Associação de certificado com publicação existente.");
                return;
            }
            var post = await postServices.GetById(payload.PostId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para associação com uma postagem inexistente. PostId: {PostId}", payload.PostId);
                return;
            }
            var certificateProjection = await certificateProjectionServices.FindWithContentBy(cp => cp.CertificateId == payload.CertificateId);
            var isNewProjection = certificateProjection == null;
            if (isNewProjection)
            {
                certificateProjection = new CertificateProjection(
                    payload.CertificateId
                );
                certificateProjection.GenerateId();
                foreach (var dataContent in payload.CertificateContents)
                {
                    if (dataContent.Id is not Guid certificateContentId)
                    {
                        this.logger.LogWarning("Evento de certificado sem identificador de conteudo válido. CertificateId: {CertificateId}", payload.CertificateId);
                        return;
                    }
                    if (dataContent.Language?.Id is not Guid languageId)
                    {
                        this.logger.LogWarning("Evento de certificado sem Language.Id válido. CertificateId: {CertificateId}", payload.CertificateId);
                        return;
                    }
                    var language = await languageServices.GetById(languageId);
                    if (language == null)
                    {
                        this.logger.LogWarning("Idioma {LanguageId} não encontrado localmente para o certificado {CertificateId}", languageId, payload.CertificateId);
                        return;
                    }
                    var certificateContentProjection = new CertificateContentProjection(
                        certificateProjection.Id,
                        certificateContentId,
                        dataContent.Title,
                        languageId
                    );
                    certificateProjection.AddCertificateContentProjection(certificateContentProjection);
                }
            }
            certificateProjection.AddPost(post);
            await unitOfWork.BeginAsync();
            try
            {
                if (isNewProjection)
                {
                    await certificateProjectionServices.Save(certificateProjection);
                }
                else
                {
                    await certificateProjectionServices.Update(certificateProjection);
                }
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task HandleCertificateDeleted(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateDeletedEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            if(payload == null)
                return;
            var certificateProjection = await certificateProjectionServices.FindWithContentBy(cp => cp.CertificateId == payload.CertificateId);
            if (certificateProjection == null)
            {
                this.logger.LogWarning("Evento recebido para certificado inexistente. CertificateId: {CertificateId}", payload.CertificateId);
                return;
            }
            await unitOfWork.BeginAsync();
            try
            {
                await certificateProjectionServices.Delete(certificateProjection);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task HandleCertificateRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateRemoveEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            var postServices = scope.ServiceProvider.GetRequiredService<IPostServices>();
            if (payload == null)
                return;
            var certificateProjection = await certificateProjectionServices.FindWithContentBy(cp => cp.CertificateId == payload.CertificateId);
            if (certificateProjection == null)
            {
                this.logger.LogWarning("Evento recebido para certificado inexistente. CertificateId: {CertificateId}", payload.CertificateId);
                return;
            }
            var post = await postServices.GetById(payload.PostId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para associação com uma postagem inexistente. PostId: {PostId}", payload.PostId);
                return;
            }
            if (!certificateProjection.Posts.Any(cp => cp.Id == post.Id))
            {
                this.logger.LogWarning("Não existe associação do certificado com a postagem. Certificate: {CertificateId}", payload.CertificateId);
                return;
            }
            certificateProjection.RemovePost(post);
            await unitOfWork.BeginAsync();
            try
            {
                await certificateProjectionServices.Update(certificateProjection);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task HandleCertificateUpdated(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateUpdatedEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            var certificateContentProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateContentProjectionServices>();
            var languageServices = scope.ServiceProvider.GetRequiredService<ILanguageServices>();
            if (payload == null)
                return;
            var certificateProjection = await certificateProjectionServices.FindWithContentBy(cp => cp.CertificateId == payload.CertificateId);
            if (certificateProjection == null)
            {
                this.logger.LogWarning("Evento recebido para certificado inexistente. CertificateId: {CertificateId}", payload.CertificateId);
                return;
            }
            foreach (var item in payload.CertificateContents)
            {
                if (item.Language?.Id is not Guid languageId)
                {
                    logger.LogWarning(
                        "Evento de certificado sem identificador de idioma válido. CertificateId: {CertificateId}",
                        payload.CertificateId);

                    return;
                }
                if (item.Id is not Guid certificateContentId)
                {
                    this.logger.LogWarning("Evento de certificado sem identificador de conteudo válido. CertificateId: {CertificateId}", payload.CertificateId);
                    return;
                }
                var certificateContentProjection = certificateProjection.CertificateContents.FirstOrDefault(ccp => ccp.LanguageId == languageId && ccp.CertificateContentId == certificateContentId);
                if (certificateContentProjection != null)
                {
                    certificateContentProjection.Update(item.Title);
                    continue;
                }
                var language = await languageServices.GetById(languageId);
                if (language == null)
                {
                    logger.LogWarning(
                        "Idioma {LanguageId} não encontrado para o certificado {CertificateId}",
                        languageId,
                        payload.CertificateId);

                    return;
                }
                var newProjection = new CertificateContentProjection(
                    certificateProjection.Id,
                    certificateContentId,
                    item.Title,
                    languageId);
                certificateProjection.AddCertificateContentProjection(newProjection);
            }
            var incomingContentIds = payload.CertificateContents
                .Select(item => new
                {
                    CertificateContentId = item.Id!.Value,
                    LanguageId = item.Language!.Id
                })
                .ToHashSet();
            var removedContents = certificateProjection.CertificateContents
                .Where(ccp => !incomingContentIds.Contains(new
                {
                    ccp.CertificateContentId,
                    ccp.LanguageId
                }))
                .ToList();
            await unitOfWork.BeginAsync();
            try
            {
                foreach (var content in removedContents)
                {
                    certificateProjection.RemoveCertificateContentProjection(content);
                    await certificateContentProjectionServices.Delete(content);
                }
                await certificateProjectionServices.Update(certificateProjection);
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