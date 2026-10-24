using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class CertificateContentProjectionRepository : Generics<CertificateContentProjection>, ICertificateContentProjectionRepository
    {
        public CertificateContentProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}