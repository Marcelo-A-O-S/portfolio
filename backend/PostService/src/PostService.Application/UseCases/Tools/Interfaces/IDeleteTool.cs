namespace PostService.Application.UseCases.Tools.Interfaces
{
    public interface IDeleteTool
    {
        Task ExecuteAsync(Guid Id);
    }
}