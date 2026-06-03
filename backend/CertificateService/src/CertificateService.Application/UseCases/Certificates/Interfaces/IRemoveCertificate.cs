using CertificateService.Application.DTOs.Requests;
namespace CertificateService.Application.UseCases.Certificates.Interfaces
{
    public interface IRemoveCertificate
    {
        Task ExecuteAsync(Guid certificateId);
    }
}