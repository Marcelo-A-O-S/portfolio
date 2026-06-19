using PostService.Domain.Entities;

namespace PostService.Domain.Interfaces
{
    public interface IMediaProjectionRepository : IGenerics<MediaProjection>
    {
        Task<MediaProjection> GetByUrl(string url);
    }
}