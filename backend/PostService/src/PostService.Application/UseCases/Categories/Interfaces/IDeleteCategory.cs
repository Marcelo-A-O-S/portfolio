namespace PostService.Application.UseCases.Categories.Interfaces
{
    public interface IDeleteCategory
    {
        Task ExecuteAsync(Guid Id);
    }
}