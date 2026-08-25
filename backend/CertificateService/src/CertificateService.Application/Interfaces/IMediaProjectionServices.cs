using CertificateService.Domain.Entities;

namespace CertificateService.Application.Interfaces
{
    public interface IMediaProjectionServices: IServices<MediaProjection>
    {
        Task<MediaProjection> GetByUrl(string url);
    }
}