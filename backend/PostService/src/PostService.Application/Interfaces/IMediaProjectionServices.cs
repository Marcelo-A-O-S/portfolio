using PostService.Domain.Entities;

namespace PostService.Application.Interfaces
{
    public interface IMediaProjectionServices : IServices<MediaProjection>
    {
        Task<MediaProjection> GetByUrl(string url);
    }
}