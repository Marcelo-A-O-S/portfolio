using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.Validations;
using PostService.Application.Validators.Interfaces;
using PostService.Application.Exceptions;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Projects
{
    public class UpdateLinkProject : IUpdateLinkProject
    {
        private readonly ILinkServices linkServices;
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IValidationServices validationServices;
        private readonly IUnitOfWork unitOfWork;
        public UpdateLinkProject(
            ILinkServices _linkServices,
            ILinkTypeServices _linkTypeServices,
            IValidationServices _validationServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkServices = _linkServices;
            this.linkTypeServices = _linkTypeServices;
            this.validationServices = _validationServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id, LinkRequest request)
        {
            ValidateLinkRequest(request);
            if(request.PostId is not Guid postId)
                throw new ValidationException("O Identificador do projeto é obrigatório.");
            await this.validationServices.ValidatePostExists(postId);
            var linkType = await GetLinkTypeAsync(request.LinkTypeId);
            var link = await GetLinkAsync(Id);
            link.SetPostId(postId);
            link.Update(request.Url, linkType.Id);
            await ProcessDescriptions(link, request.Descriptions);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.linkServices.Update(link);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
        private static void ValidateLinkRequest(LinkRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
        private async Task<LinkType> GetLinkTypeAsync(Guid linkTypeId)
        {
            var linkType = await this.linkTypeServices.GetById(linkTypeId);
            if(linkType == null)
                throw new NotFoundException("Tipo de link não encontrado.");
            return linkType;
        }
        private async Task<Link> GetLinkAsync(Guid linkId)
        {
            var link = await this.linkServices.GetFullDataById(linkId);
            if(link == null)
                throw new NotFoundException("Link não encontrado.");
            return link;
        }
        private async Task ProcessDescriptions(Link link, List<LinkDescriptionRequest> linkDescriptionRequests)
        {
            var requestLinkDescriptionIds = linkDescriptionRequests
                .Where(c => c.Id.HasValue)
                .Select(c => c.Id!.Value);
            link.ValidateDescriptions(requestLinkDescriptionIds);
            foreach (var item in linkDescriptionRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                    throw new ValidationException($"Erro ao validar dados: {validationError}");
                await this.validationServices.ValidateLanguageExists(item.LanguageId);
                if (item.Id.HasValue)
                {
                    var linkDescription = link.Descriptions.FirstOrDefault(tc => tc.Id == item.Id.Value);
                    if (linkDescription == null)
                        throw new NotFoundException("Descrição de link não encontrada.");
                    linkDescription.Update(item.Title);
                }
                else
                {
                    var linkDescription = new LinkDescription(link.Id, item.LanguageId, item.Title);
                    link.AddLinkDescription(linkDescription);
                }
            }
        }
    }
}