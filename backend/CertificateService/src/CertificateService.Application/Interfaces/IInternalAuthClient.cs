namespace CertificateService.Application.Interfaces
{
    public interface IInternalAuthClient
    {
        Task<string> GetToken();
    }
}