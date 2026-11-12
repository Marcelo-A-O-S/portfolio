using MediaService.Domain.Entities;

namespace MediaService.Application.Interfaces
{
    public interface IMediaDeletionServices : IServices<MediaDeletion>
    {
        Task<List<MediaDeletion>> GetMediaPending();
    }
}