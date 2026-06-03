namespace CertificateService.Application.UseCases.CertificatePosts.Interfaces
{
    public interface IAddRelatePost
    {
        Task ExecuteAsync(Guid certificateId, Guid postId);
    }
}