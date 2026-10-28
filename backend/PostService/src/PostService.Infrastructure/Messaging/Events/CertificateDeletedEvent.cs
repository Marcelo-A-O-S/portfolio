using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Infrastructure.Messaging.Events
{
    public class CertificateDeletedEvent
    {
        public Guid CertificateId { get; set; }
    }
}