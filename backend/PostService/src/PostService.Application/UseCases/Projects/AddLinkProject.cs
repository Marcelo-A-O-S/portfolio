using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.Validators.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using PostService.Application.Exceptions;

namespace PostService.Application.UseCases.Projects
{
    public class AddLinkProject : IAddLinkProject
    {
        private readonly ILinkServices linkServices;
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IValidationServices validationServices;
        private readonly IUnitOfWork unitOfWork;
        public AddLinkProject(
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
            if(request.PostId is not Guid postId)
                throw new ValidationException("O Identificador do projeto é obrigatório.");
            await this.validationServices.ValidatePostExists(postId);
            var linkType = await GetLinkTypeAsync(request.LinkTypeId);
            var link = new Link(request.Url, linkType.Id);
            await ProcessDescriptions(link, request.Descriptions);
            link.SetPostId(postId);
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