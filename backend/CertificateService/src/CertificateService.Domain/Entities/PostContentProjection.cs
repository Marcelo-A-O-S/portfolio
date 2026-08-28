using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateService.Domain.Entities
{
    public class PostContentProjection
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public LanguageProjection Language { get; private set; }
        public MediaProjection MediaProjection { get; private set; }
    }
}