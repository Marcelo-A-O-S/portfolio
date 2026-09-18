namespace CertificateService.Application.Interfaces
{
    public interface IAuthServicesClient
    {
        Task<string> GetToken();
    }
}