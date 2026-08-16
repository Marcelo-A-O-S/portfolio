using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.Validations;
using PostService.Application.Validators.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Tools
{
    public class UpdateLinkTool : IUpdateLinkTool
    {
        private readonly ILinkServices linkServices;
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IToolsServices toolsServices;
        private readonly IToolValidationServices toolValidationServices;
        private readonly IUnitOfWork unitOfWork;
        public UpdateLinkTool(
            ILinkServices _linkServices,
            ILinkTypeServices _linkTypeServices,
            IToolsServices _toolsServices,
            IToolValidationServices _toolValidationServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkServices = _linkServices;
            this.linkTypeServices = _linkTypeServices;
            this.toolsServices = _toolsServices;
            this.toolValidationServices = _toolValidationServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id, LinkRequest request)
        {
            await ValidateLinkRequest(request);
            if(request.ToolId is not Guid toolId)
                throw new ValidationException("O Identificador da ferramenta é obrigatória.");
            await this.toolValidationServices.ValidateToolExists(toolId);
            var linkType = await GetLinkTypeAsync(request.LinkTypeId);
            var link = await GetLinkAsync(Id);
            link.SetToolId(toolId);
            link.Update(request.Url, request.Title, linkType.Id);
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
        private async Task<Link> GetLinkAsync(Guid linkId)
        {
            var link = await this.linkServices.GetById(linkId);
            if(link == null)
                throw new NotFoundException("Link não encontrado.");
            return link;
        }
    }
}