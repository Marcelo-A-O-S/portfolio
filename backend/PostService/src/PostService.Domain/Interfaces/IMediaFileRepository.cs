using PostService.Domain.Entities;

namespace PostService.Domain.Interfaces
{
    public interface IMediaFileRepository : IGenerics<MediaFile>
    {
        Task<MediaFile> GetByPath(string path);
    }
}