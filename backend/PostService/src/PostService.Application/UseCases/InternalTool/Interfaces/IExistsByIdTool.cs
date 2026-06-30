namespace PostService.Application.UseCases.InternalTool.Interfaces
{
    public interface IExistsByIdTool
    {
        Task<bool> ExecuteAsync(Guid Id);
    }
}