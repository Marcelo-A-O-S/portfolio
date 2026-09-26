using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Domain.Interfaces;

namespace CertificateService.Application.UseCases.Certificates
{
    public class RemovePostProjectionCertificate : IRemovePostProjectionCertificate
    {
        private readonly IPostProjectionServices postProjectionServices;
        private readonly IValidationServices validationServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IUnitOfWork unitOfWork;
        public RemovePostProjectionCertificate(
            IPostProjectionServices _postProjectionServices,
            IValidationServices _validationServices,
            IRabbitMQProducer _rabbitMQProducer,
            IUnitOfWork _unitOfWork
        )
        {
            this.postProjectionServices = _postProjectionServices;
            this.validationServices = _validationServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid certificateId, Guid postProjectionId)
        {
            var postProjection = await this.postProjectionServices.FindBy()
            await this.unitOfWork.BeginAsync();
            try
            {

            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
            throw new NotImplementedException();
        }
    }
}