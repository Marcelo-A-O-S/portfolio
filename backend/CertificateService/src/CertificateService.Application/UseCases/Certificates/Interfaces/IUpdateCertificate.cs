using CertificateService.Application.DTOs.Requests;
namespace CertificateService.Application.UseCases.Certificates.Interfaces
{
    public interface IUpdateCertificate
    {
        Task ExecuteAsync(Guid certificateId, CertificateRequest certificateRequest);
    }
}