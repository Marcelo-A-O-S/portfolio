using MediaService.Domain.Entities;
namespace MediaService.Domain.Interfaces
{
    public interface IMediaRepository : IGenerics<Media>
    {
        Task<Media> GetByPath(string path);
        Task<List<Media>> ListExpiredUncommittedMediaAsync();
    }
}