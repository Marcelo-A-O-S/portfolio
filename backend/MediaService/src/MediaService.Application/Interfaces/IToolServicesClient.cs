namespace MediaService.Application.Interfaces
{
    public interface IToolServicesClient
    {
        Task<bool> ToolExistsAsync(Guid toolId);
    }
}