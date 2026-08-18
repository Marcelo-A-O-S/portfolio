using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Domain.Entities;
using PostService.Application.Interfaces;
using PostService.Application.Exceptions;

namespace PostService.Application.UseCases.Projects
{
    public class DeleteLinkProject : IDeleteLinkProject
    {
        private readonly ILinkServices linkServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteLinkProject(
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