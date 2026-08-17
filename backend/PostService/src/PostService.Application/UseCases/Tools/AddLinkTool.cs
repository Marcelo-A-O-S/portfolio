using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.Validations;
using PostService.Application.Validators.Interfaces;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Tools
{
    public class AddLinkTool : IAddLinkTool
    {
        private readonly ILinkServices linkServices;
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IValidationServices validationServices;
        private readonly IUnitOfWork unitOfWork;
        public AddLinkTool(
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
        public async Task ExecuteAsync(LinkRequest request)
        {
            await ValidateLinkRequest(request);
            if(request.ToolId is not Guid toolId)
                throw new ValidationException("O Identificador da ferramenta é obrigatória.");
            await this.validationServices.ValidateToolExists(toolId);
            var linkType = await GetLinkTypeAsync(request.LinkTypeId);
            var link = new Link(request.Url, linkType.Id);
            await ProcessDescriptions(link, request.Descriptions);
            link.SetToolId(toolId);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.linkServices.Save(link);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task ValidateLinkRequest(LinkRequest request)
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
        private async Task ProcessDescriptions(Link link, List<LinkDescriptionRequest> descriptionRequests)
        {
            foreach (var item in descriptionRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                    throw new ValidationException($"Erro ao validar dados: {validationError}");
                await this.validationServices.ValidateLanguageExists(item.LanguageId);
                var linkDescription = new LinkDescription(link.Id, item.LanguageId, item.Title);
                link.AddLinkDescription(linkDescription);
            }
        }
    }
}