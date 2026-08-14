using PostService.Application.Interfaces;
using PostService.Application.UseCases.LinkTypes.Interfaces;
using PostService.Domain.Entities;
using PostService.Application.Exceptions;
namespace PostService.Application.UseCases.LinkTypes
{
    public class DeleteLinkType : IDeleteLinkType
    {
        private readonly ILinkTypeServices linkTypeServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteLinkType(
            ILinkTypeServices _linkTypeServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkTypeServices = _linkTypeServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var linkType = await GetLinkTypeAsync(Id);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.linkTypeServices.Delete(linkType);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task<LinkType> GetLinkTypeAsync(Guid Id)
        {
            var linkType = await this.linkTypeServices.GetById(Id);
            if(linkType == null)
                throw new NotFoundException("Tipo de Link não encontrado.");
            return linkType;
        }
    }
}