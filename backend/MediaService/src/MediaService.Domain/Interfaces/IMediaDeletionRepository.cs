using MediaService.Domain.Entities;
namespace MediaService.Domain.Interfaces
{
    public interface IMediaDeletionRepository : IGenerics<MediaDeletion>
    {
        Task<List<MediaDeletion>> GetMediaPending();
    }
}