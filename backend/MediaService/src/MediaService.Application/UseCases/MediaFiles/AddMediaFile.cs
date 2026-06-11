using MediaService.Application.DTOs.Requests;
using MediaService.Application.UseCases.MediaFiles.Interfaces;
namespace MediaService.Application.UseCases.MediaFiles
{
    public class AddMediaFile : IAddMediaFile
    {
        public Task ExecuteAsync(MediaFileRequest mediaFileRequest)
        {
            throw new NotImplementedException();
        }
    }
}