using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Domain.Interfaces;
using CertificateService.Application.Exceptions;
using System.Runtime.ConstrainedExecution;
using CertificateService.Domain.Entities;
namespace CertificateService.Application.UseCases.Certificates
{
    public class RemovePostProjectionCertificate : IRemovePostProjectionCertificate
    {
        private readonly IPostProjectionServices postProjectionServices;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public RemovePostProjectionCertificate(
            IPostProjectionServices _postProjectionServices,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.postProjectionServices = _postProjectionServices;
            this.outboxMessageServices = _outboxMessageServices;
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
                var outboxMessage = new OutboxMessage(
                    "PostCertificateRemoved",
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        PostId = postProjection.PostId,
                        CertificateId = postProjection.CertificateId
                    })
                );
                await this.outboxMessageServices.Save(outboxMessage);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}