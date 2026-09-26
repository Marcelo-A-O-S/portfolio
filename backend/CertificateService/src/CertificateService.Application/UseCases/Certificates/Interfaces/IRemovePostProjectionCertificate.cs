using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateService.Application.UseCases.Certificates.Interfaces
{
    public interface IRemovePostProjectionCertificate
    {
        Task ExecuteAsync(Guid certificateId, Guid postId);
    }
}