using Microsoft.AspNetCore.Http;
namespace PostService.Application.Interfaces
{
    public interface IFileServices
    {
        Task<string?> SaveImageAsync(IFormFile file, string folder);
        void DeleteImage(string imgUrl);
    }
}