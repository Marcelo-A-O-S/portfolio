namespace PostService.Application.UseCases.Tools.Interfaces
{
    public interface IDeleteLinkTool
    {
        Task ExecuteAsync(Guid Id);
    }
}