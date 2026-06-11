using MediaService.Domain.Entities;
using Microsoft.AspNetCore.Http;
namespace MediaService.Application.Interfaces
{
    public interface IMediaFileServices : IServices<MediaFile>
    {
        Task<MediaFile> SaveImageAsync(Guid ownerId, string owner, IFormFile file, string folder);
        Task DeleteImageAsync(MediaFile mediaFile);
        Task<MediaFile> GetByPath(string path);
    }
}