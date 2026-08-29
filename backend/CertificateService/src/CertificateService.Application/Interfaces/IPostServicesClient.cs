using CertificateService.Application.DTOs.Responses;
namespace CertificateService.Application.Interfaces
{
    public interface IPostServicesClient
    {
        Task<bool> PostExistsAsync(Guid postId);
        Task<PostResponse> GetPostAsync(Guid postId);
    }
}