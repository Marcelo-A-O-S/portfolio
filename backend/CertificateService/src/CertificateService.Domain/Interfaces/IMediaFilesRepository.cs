using CertificateService.Domain.Entities;

namespace CertificateService.Domain.Interfaces
{
    public interface IMediaFilesRepository : IGenerics<MediaFile>
    {
        Task<MediaFile> GetByPath(string path);
    }
}