namespace CertificateService.Application.Interfaces
{
    public interface ICertificatePostsCacheServices
    {
        Task AddCertificatePostCache(string key, Guid certificatePostId);
        Task<string?> GetCertificatePostCache(string key);
        Task RemoveCertificatePostCache(string key);
    }
}