namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface IDeleteProject
    {
        Task ExecuteAsync(Guid Id);
    }
}