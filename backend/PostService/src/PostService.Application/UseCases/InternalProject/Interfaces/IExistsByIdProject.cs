namespace PostService.Application.UseCases.InternalProject.Interfaces
{
    public interface IExistsByIdProject
    {
        Task<bool> ExecuteAsync(Guid Id);
    }
}