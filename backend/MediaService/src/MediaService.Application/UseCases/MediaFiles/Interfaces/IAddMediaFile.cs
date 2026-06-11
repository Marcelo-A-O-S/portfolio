using MediaService.Application.DTOs.Requests;
namespace MediaService.Application.UseCases.MediaFiles.Interfaces
{
    public interface IAddMediaFile
    {
        Task ExecuteAsync(MediaFileRequest mediaFileRequest);
    }
}