using MediaService.Domain.Entities;

namespace MediaService.Domain.Interfaces
{
    public interface IMediaFileRepository : IGenerics<MediaFile>
    {
        Task<MediaFile> GetByPath(string path);
        Task DeleteExpiredPendingMediaAsync();
    }
}