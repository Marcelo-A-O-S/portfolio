using CertificateService.Application.DTOs.Requests;
namespace CertificateService.Application.UseCases.Certificates.Interfaces
{
    public interface IAddCertificate
    {
        Task ExecuteAsync(CertificateRequest certificateRequest);
    }
}