using MediaService.Application.DTOs.Requests;
using MediaService.Application.DTOs.Responses;
using MediaService.Application.Exceptions;
using MediaService.Application.Interfaces;
using MediaService.Application.UseCases.MediaFiles.Interfaces;
using MediaService.Application.Validations;
using MediaService.Application.Validators.Interfaces;
using MediaService.Application.Constants;
namespace MediaService.Application.UseCases.MediaFiles
{
    public class AddMediaFile : IAddMediaFile
    {
        private readonly IMediaFileServices mediaFileServices;
        public AddMediaFile(
            IMediaFileServices _mediaFileServices
        )
        {
            this.mediaFileServices = _mediaFileServices;
        }
        public async Task<MediaFileResponse> ExecuteAsync(MediaFileRequest mediaFileRequest)
        {
            ValidateRequest(mediaFileRequest);
            var folder = FolderResolver.Resolve(
                mediaFileRequest.OwnerType
            );
            var media = await this.mediaFileServices.SaveImageAsync(
                mediaFileRequest.OwnerId,
                mediaFileRequest.OwnerType,
                mediaFileRequest.File,
                folder
            );
            return new MediaFileResponse
            {
                Id = media.Id,
                Path = media.Path
            };
        }
        private static void ValidateRequest(MediaFileRequest mediaFileRequest)
        {
            var validationError = ValidationHelper.Validate(mediaFileRequest);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
    }
}