namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface IDeleteLinkProject
    {
        Task ExecuteAsync(Guid Id);
    }
}