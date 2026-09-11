using PostService.Domain.Entities;
namespace PostService.Application.Interfaces
{
    public interface IAuthorServices : IServices<Author>
    {
        Task<Author> GetByUserId(Guid userId);
    }
}