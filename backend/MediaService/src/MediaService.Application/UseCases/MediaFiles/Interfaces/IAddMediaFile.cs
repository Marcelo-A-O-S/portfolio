using MediaService.Application.DTOs.Requests;
using MediaService.Application.DTOs.Responses;
namespace MediaService.Application.UseCases.MediaFiles.Interfaces
{
    public interface IAddMediaFile
    {
        Task<MediaFileResponse> ExecuteAsync(MediaFileRequest mediaFileRequest);
    }
}