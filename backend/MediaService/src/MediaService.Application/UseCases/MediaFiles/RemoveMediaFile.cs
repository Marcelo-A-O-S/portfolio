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
        private readonly IMediaStorage mediaStorage;
        private readonly IUnitOfWork unitOfWork;
        public RemoveMediaFile(
            IMediaServices _mediaFileServices,
            IMediaOwnerServices _mediaOwnerServices,
            IMediaStorage _mediaStorage,
            IUnitOfWork _unitOfWork
        )
        {
            this.mediaFileServices = _mediaFileServices;
            this.mediaOwnerServices = _mediaOwnerServices;
            this.mediaStorage = _mediaStorage;
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
            var isMediaDeleted = false;
            try
            {
                if (removeOwner != null)
                {
                    media.RemoveOwner(removeOwner);
                    await mediaOwnerServices.Delete(removeOwner);
                }
                if(media.MediaOwners.Count == 0)
                {
                    await this.mediaFileServices.Delete(media);
                    isMediaDeleted = true;
                }
                await this.unitOfWork.CommitAsync();
                if (isMediaDeleted)
                {
                    await this.mediaStorage.DeleteAsync(media.Path);
                }
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
                mo.OwnerType == request.OwnerType));
            if(media == null)
                throw new NotFoundException("Midia não encontrada");
            return media;
        }
    }
}