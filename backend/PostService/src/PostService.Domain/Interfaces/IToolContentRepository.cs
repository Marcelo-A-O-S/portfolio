using PostService.Domain.Entities;
namespace PostService.Domain.Interfaces
{
    public interface IToolContentRepository : IGenerics<ToolContent>
    {
        Task<ToolContent> GetByToolId(Guid toolId);
    }
}