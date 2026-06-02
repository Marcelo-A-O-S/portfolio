using CertificateService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CertificateService.Application.Interfaces
{
    public interface IMediaFilesServices : IServices<MediaFile>
    {
        Task<MediaFile?> SaveImageAsync(IFormFile file, string folder, bool isCommitted = false);
        Task DeleteImageAsync(MediaFile mediaFile);
        Task<MediaFile> GetByPath(string path);
    }
}