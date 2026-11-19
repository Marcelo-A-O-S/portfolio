using MediaService.Application.DTOs.Requests;
using MediaService.Application.Exceptions;
using MediaService.Application.Interfaces;
using MediaService.Application.UseCases.MediaFiles.Interfaces;
using MediaService.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
namespace MediaService.Application.UseCases.MediaFiles
{
    public class RemoveMediaFile : IRemoveMediaFile
    {
        private readonly IMediaServices mediaFileServices;
        private readonly IMediaOwnerServices mediaOwnerServices;
        private readonly IMediaDeletionServices mediaDeletionServices;
        private readonly IUnitOfWork unitOfWork;
        public RemoveMediaFile(
            IMediaServices _mediaFileServices,
            IMediaOwnerServices _mediaOwnerServices,
            IMediaDeletionServices _mediaDeletionServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.mediaFileServices = _mediaFileServices;
            this.mediaOwnerServices = _mediaOwnerServices;
            this.mediaDeletionServices = _mediaDeletionServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(MediaDeleteRequest request)
        {
            var media = await GetMediaFile(request);
            await this.unitOfWork.BeginAsync();
            var removeOwner = media.MediaOwners
                .Where(m => m.MediaId == request.MediaId && 
                m.OwnerId == request.OwnerId &&
                m.OwnerType == request.OwnerType)
                .FirstOrDefault();

            try
            {
                if (removeOwner != null)
                {
                    media.RemoveOwner(removeOwner);
                    await mediaOwnerServices.Delete(removeOwner);
                }
                if(media.MediaOwners.Count == 0)
                {
                    var mediaDeletion = new MediaDeletion(
                        media.Path
                    );
                    await this.mediaFileServices.Delete(media);
                    await this.mediaDeletionServices.Save(mediaDeletion);
                }
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task<Media> GetMediaFile(MediaDeleteRequest request)
        {
            var media = await this.mediaFileServices.GetMediaBy(m => 
                m.Id == request.MediaId && 
                m.MediaOwners.Any(mo => 
                mo.MediaId == request.MediaId && 
                mo.OwnerId == request.OwnerId &&
                mo.OwnerType == request.OwnerType));
            if(media == null)
                throw new NotFoundException("Midia não encontrada");
            return media;
        }
    }
}