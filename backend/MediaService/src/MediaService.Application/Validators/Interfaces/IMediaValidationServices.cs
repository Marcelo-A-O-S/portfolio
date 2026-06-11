namespace MediaService.Application.Validators.Interfaces
{
    public interface IMediaValidationServices
    {
        Task ValidatePostExists(Guid postId);
        Task ValidateToolExists(Guid toolId);
        Task ValidateCertificateExists(Guid certificateId);
        Task ValidateOwnerExists(Guid ownerId, string ownerType);
    }
}