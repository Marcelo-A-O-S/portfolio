using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.LinkTypes.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.LinkTypes
{
    public class UpdateLinkType : IUpdateLinkType
    {
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IUnitOfWork unitOfWork;
        public UpdateLinkType(
            ILinkTypeServices _linkTypeServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkTypeServices = _linkTypeServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id, LinkTypeRequest request)
        {
            await ValidateLinkTypeRequest(request);
            var linkType = await GetLinkTypeAsync(Id);
            linkType.Update(request.Name, request.BackgroundColor, request.TextColor, request.BorderColor, request.Icon);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.linkTypeServices.Update(linkType);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task ValidateLinkTypeRequest(LinkTypeRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
        private async Task<LinkType> GetLinkTypeAsync(Guid Id)
        {
            var linkType = await this.linkTypeServices.GetById(Id);
            if (linkType == null)
                throw new NotFoundException("Tipo de Link não encontrado.");
            return linkType;
        }
    }
}