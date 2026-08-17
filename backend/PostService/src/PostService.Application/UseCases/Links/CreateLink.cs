using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Links.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Links
{
    public class CreateLink : ICreateLink
    {
        private readonly ILinkServices linkServices;
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IUnitOfWork unitOfWork;
        public CreateLink(
            ILinkServices _linkServices,
            ILinkTypeServices _linkTypeServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkServices = _linkServices;
            this.linkTypeServices = _linkTypeServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(LinkRequest request)
        {
            await ValidateLinkRequest(request);
            var linkType = await GetLinkTypeAsync(request.LinkTypeId);
            var link = new Link(request.Url, linkType.Id);
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
    }
}