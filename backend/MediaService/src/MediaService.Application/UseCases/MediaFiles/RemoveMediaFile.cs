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
        public RemoveMediaFile(
            IMediaServices _mediaFileServices
        )
        {
            this.mediaFileServices = _mediaFileServices;
        }
        public async Task ExecuteAsync(Guid mediaFileId)
        {
            var mediaFile = await GetMediaFile(mediaFileId);
            await this.mediaFileServices.DeleteById(mediaFile.Id);
        }
        private async Task<Media> GetMediaFile(Guid mediaFileId)
        {
            var media = await this.mediaFileServices.GetById(mediaFileId);
            if(media == null)
                throw new NotFoundException("Midia não encontrada");
            return media;
        }
    }
}