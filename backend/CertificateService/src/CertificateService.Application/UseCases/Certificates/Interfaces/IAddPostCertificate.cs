namespace CertificateService.Application.UseCases.Certificates.Interfaces
{
    public interface IAddPostProjectionCertificate
    {
        Task ExecuteAsync(Guid certificateId, Guid postId);
    }
}