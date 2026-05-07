using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Categories.Interfaces
{
    public interface ICreateCategory
    {
        Task ExecuteAsync(CategoryRequest categoryRequest);
    }
}