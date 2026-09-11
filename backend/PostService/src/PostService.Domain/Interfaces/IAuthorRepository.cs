using PostService.Domain.Entities;
namespace PostService.Domain.Interfaces
{
    public interface IAuthorRepository : IGenerics<Author>
    {
        Task<Author> GetByUserId(Guid userId);
    }
}