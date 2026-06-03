namespace CertificateService.Application.UseCases.CertificatePosts.Interfaces
{
    public interface IRemoveRelatePost
    {
        Task ExecuteAsync(Guid certificatePostId);
    }
}