using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateService.Application.Configurations
{
    public class InternalClientOptions
    {
        public Dictionary<string, InternalClient> InternalClients { get; set; } = [];
    }
}