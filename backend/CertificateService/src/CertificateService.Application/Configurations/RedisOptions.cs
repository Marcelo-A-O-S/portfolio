using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateService.Application.Configurations
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
    }
}