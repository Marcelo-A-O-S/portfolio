using System.ComponentModel.DataAnnotations;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.LinkTypes.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.LinkTypes
{
    public class CreateLinkType : ICreateLinkType
    {
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IUnitOfWork unitOfWork;
        public CreateLinkType(
            ILinkTypeServices _linkTypeServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkTypeServices = _linkTypeServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(LinkTypeRequest request)
        {
            await ValidateLinkTypeRequest(request);
            var linkType = new LinkType(request.Name, request.BackgroundColor, request.TextColor, request.Icon);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.linkTypeServices.Save(linkType);
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
    }
}