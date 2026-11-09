using MediaService.Application.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace MediaService.Application.Interfaces
{
    public interface IMediaStorage
    {
        Task<TemporaryMediaFile> SaveTemporaryAsync(
            IFormFile file,
            string folder
        );
        Task DeleteTemporaryAsync(string temporaryPath);
        Task MoveToFinalLocationAsync(
            string temporaryPath,
            string finalPath
        );
        Task DeleteAsync(string path);
    }
}