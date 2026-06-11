namespace MediaService.Application.Interfaces
{
    public interface ICertificateServicesClient
    {
        Task<bool> CertificateExistsAsync(Guid certificateId);
    }
}