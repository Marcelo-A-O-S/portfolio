using PostService.Domain.Entities;
using Microsoft.AspNetCore.Http;
namespace PostService.Application.Interfaces
{
    public interface IMediaFileServices : IServices<MediaFile>
    {
        Task<MediaFile?> SaveImageAsync(IFormFile file, string folder, bool isCommitted = false);
        Task DeleteImageAsync(MediaFile mediaFile);
        Task<MediaFile> GetByPath(string path);
    }
}