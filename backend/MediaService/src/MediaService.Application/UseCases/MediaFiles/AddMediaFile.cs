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
        private readonly IMediaValidationServices mediaValidationServices;
        private readonly IMediaFileServices mediaFileServices;
        public AddMediaFile(
            IMediaValidationServices _mediaValidationServices,
            IMediaFileServices _mediaFileServices
        )
        {
            this.mediaValidationServices = _mediaValidationServices;
            this.mediaFileServices = _mediaFileServices;
        }
        public async Task<MediaFileResponse> ExecuteAsync(MediaFileRequest mediaFileRequest)
        {
            ValidateRequest(mediaFileRequest);
            await this.mediaValidationServices.ValidateOwnerExists(mediaFileRequest.OwnerId, mediaFileRequest.OwnerType);
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