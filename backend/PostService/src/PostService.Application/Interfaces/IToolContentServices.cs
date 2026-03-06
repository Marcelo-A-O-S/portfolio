using PostService.Domain.Entities;

namespace PostService.Application.Interfaces
{
    public interface IToolContentServices : IServices<ToolContent>
    {
        Task<ToolContent> GetByToolId(Guid toolId);
    }
}