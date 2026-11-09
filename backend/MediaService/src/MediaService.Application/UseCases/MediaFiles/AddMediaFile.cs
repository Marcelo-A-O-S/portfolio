using MediaService.Application.DTOs.Requests;
using MediaService.Application.DTOs.Responses;
using MediaService.Application.Exceptions;
using MediaService.Application.Interfaces;
using MediaService.Application.UseCases.MediaFiles.Interfaces;
using MediaService.Application.Validations;
using MediaService.Application.Constants;
using MediaService.Domain.Entities;
using Microsoft.Extensions.Logging;
namespace MediaService.Application.UseCases.MediaFiles
{
    public class AddMediaFile : IAddMediaFile
    {
        private readonly IMediaServices mediaFileServices;
        private readonly IMediaStorage mediaStorage;
        private readonly IMediaOwnerServices mediaOwnerServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AddMediaFile> logger;
        public AddMediaFile(
            IMediaServices _mediaFileServices,
            IMediaStorage _mediaStorage,
            IMediaOwnerServices _mediaOwnerServices,
            IUnitOfWork _unitOfWork,
            ILogger<AddMediaFile> _logger
        )
        {
            this.mediaFileServices = _mediaFileServices;
            this.mediaStorage = _mediaStorage;
            this.mediaOwnerServices = _mediaOwnerServices;
            this.unitOfWork = _unitOfWork;
            this.logger = _logger;
        }
        public async Task<MediaFileResponse> ExecuteAsync(MediaFileRequest request)
        {
            ValidateRequest(request);
            var folder = FolderResolver.Resolve(
                request.OwnerType
            );
            var temporaryFile = await this.mediaStorage.SaveTemporaryAsync(request.File, folder);
            var fileMoved = false;
            var transactionStarted = false;
            try
            {
                await this.unitOfWork.BeginAsync();
                transactionStarted = true;
                var media = new Media(
                    temporaryFile.FinalPath,
                    temporaryFile.ContentType,
                    temporaryFile.Length
                );
                media.GenerateId();
                var mediaOwner = new MediaOwner(
                    media.Id,
                    request.OwnerId,
                    request.OwnerType
                );
                await this.mediaFileServices.Save(media);
                await this.mediaOwnerServices.Save(mediaOwner);
                await this.mediaStorage.MoveToFinalLocationAsync(temporaryFile.TemporaryPath, temporaryFile.FinalPath);
                fileMoved = true;
                media.MarkAsUploaded();
                await this.unitOfWork.CommitAsync();
                return new MediaFileResponse
                {
                    MediaId = media.Id,
                    Url = media.Path,
                    OwnerType = mediaOwner.OwnerType
                };
            }
            catch
            {
                if (transactionStarted)
                {
                    try
                    {
                        await unitOfWork.RollbackAsync();
                    }
                    catch
                    {
                        this.logger.LogWarning("Falha no rollback.");
                    }
                }
                if (fileMoved)
                {
                    
                    try
                    {
                        await this.mediaStorage.DeleteAsync(temporaryFile.FinalPath);
                    }
                    catch
                    {
                        this.logger.LogWarning("Falha na exclusão do arquivo definitivo");
                    }
                }
                try
                {
                    await this.mediaStorage.DeleteTemporaryAsync(temporaryFile.TemporaryPath);
                }
                catch
                {
                    this.logger.LogWarning("Falha na exclusão do arquivo temporário.");
                }
                throw;
            }
        }
        private static void ValidateRequest(MediaFileRequest mediaFileRequest)
        {
            var validationError = ValidationHelper.Validate(mediaFileRequest);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
    }
}