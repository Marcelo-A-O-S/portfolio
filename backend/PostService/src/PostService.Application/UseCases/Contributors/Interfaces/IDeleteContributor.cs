namespace PostService.Application.UseCases.Contributors.Interfaces
{
    public interface IDeleteContributor
    {
        Task ExecuteAsync(Guid Id);
    }
}