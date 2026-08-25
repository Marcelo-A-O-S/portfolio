using CertificateService.Domain.Entities;
namespace CertificateService.Domain.Interfaces
{
    public interface IMediaProjectionRepository : IGenerics<MediaProjection>
    {
        Task<MediaProjection> GetByUrl(string url);
    }
}