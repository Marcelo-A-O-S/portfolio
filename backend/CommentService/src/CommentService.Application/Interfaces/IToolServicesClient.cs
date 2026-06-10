namespace CommentService.Application.Interfaces
{
    public interface IToolServicesClient
    {
        Task<bool> ToolExistsAsync(Guid toolId);
    }
}