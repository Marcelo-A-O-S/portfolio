using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.UseCases.Links.Interfaces;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Application.Exceptions;

namespace PostService.Application.UseCases.Tools
{
    public class DeleteLinkTool : IDeleteLinkTool
    {
        private readonly ILinkServices linkServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteLinkTool(
            ILinkServices _linkServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.linkServices = _linkServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var link = await GetLinkAsync(Id);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.linkServices.Delete(link);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
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