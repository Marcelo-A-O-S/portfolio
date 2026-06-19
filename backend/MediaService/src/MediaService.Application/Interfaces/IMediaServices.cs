using MediaService.Domain.Entities;
using Microsoft.AspNetCore.Http;
namespace MediaService.Application.Interfaces
{
    public interface IMediaServices : IServices<Media>
    {
        Task<Media> SaveImageAsync(Guid? ownerId, string ownerType, IFormFile file, string folder);
        Task DeleteImageAsync(Media mediaFile);
        Task<Media> GetByPath(string path);
        Task<List<Media>> ListExpiredUncommittedMediaAsync();
    }
}