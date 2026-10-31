using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Domain.Interfaces;
using CertificateService.Application.Exceptions;
using System.Runtime.ConstrainedExecution;
using CertificateService.Domain.Entities;
using System.Text.Json;
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
            var postProjection = await this.postProjectionServices.GetAllBy(pp => pp.Id == postProjectionId && pp.Certificates.Any(c => c.Id == certificateId));
            if(postProjection == null)
                throw new NotFoundException("Relacionamento entre certificado e projeto não encontrado!");
            await this.unitOfWork.BeginAsync();
            try
            {
                var certificate = postProjection.Certificates.FirstOrDefault(c => c.Id == certificateId);
                if(certificate != null)
                {
                    postProjection.RemoveCertificate(certificate);
                }
                await this.postProjectionServices.Update(postProjection);
                var outboxMessage = new OutboxMessage(
                    "PostCertificateRemoved",
                    JsonSerializer.Serialize(new
                    {
                        PostId = postProjection.PostId,
                        CertificateId = certificateId
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