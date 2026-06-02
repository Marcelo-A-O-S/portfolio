using System.ComponentModel.DataAnnotations;
using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validations;

namespace CertificateService.Application.UseCases.Certificates
{
    public class RemoveCertificate : IRemoveCertificate
    {
        public RemoveCertificate()
        {
            
        }
        public async Task ExecuteAsync(Guid certificateId, CertificateRequest certificateRequest)
        {
            ValidateRequest(certificateRequest);
        }
        private static void ValidateRequest(CertificateRequest certificateRequest)
        {
            var validationError = ValidationHelper.Validate(certificateRequest);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
    }
}