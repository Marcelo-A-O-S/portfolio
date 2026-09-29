using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Domain.Interfaces;
using CertificateService.Application.Exceptions;
using System.Runtime.ConstrainedExecution;
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
            var postProjection = await this.postProjectionServices.FindBy(pp => pp.Id == postProjectionId && pp.CertificateId == certificateId);
            if(postProjection == null)
                throw new NotFoundException("Relacionamento entre certificado e projeto não encontrado!");
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.postProjectionServices.DeleteById(postProjection.Id);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
            await this.rabbitMQProducer.Publish("RemovePostCertificate", new
            {
                PostId = postProjection.PostId,
                CertificateId = postProjection.CertificateId
            });
        }
    }
}