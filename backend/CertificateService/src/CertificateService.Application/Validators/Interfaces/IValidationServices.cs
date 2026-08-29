namespace CertificateService.Application.Validators.Interfaces
{
    public interface IValidationServices
    {
        Task ValidatePostExists(Guid postId);
        Task ValidateCertificateExists(Guid certificateId);
    }
}