using System.Linq.Expressions;
using MediaService.Domain.Entities;
namespace MediaService.Domain.Interfaces
{
    public interface IMediaRepository : IGenerics<Media>
    {
        Task<Media> GetByPath(string path);
        Task<List<Media>> ListExpiredUncommittedMediaAsync();
        Task<Media> GetMediaBy(Expression<Func<Media,bool>> predicate);
    }
}