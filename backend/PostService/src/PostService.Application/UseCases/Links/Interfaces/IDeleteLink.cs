namespace PostService.Application.UseCases.Links.Interfaces
{
    public interface IDeleteLink
    {
        Task ExecuteAsync(Guid Id);
    }
}