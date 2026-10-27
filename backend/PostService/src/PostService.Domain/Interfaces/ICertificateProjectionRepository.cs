using System.Runtime.ConstrainedExecution;
using PostService.Domain.Entities;
using System.Linq.Expressions;
namespace PostService.Domain.Interfaces
{
    public interface ICertificateProjectionRepository : IGenerics<CertificateProjection>
    {
        Task<CertificateProjection> FindWithContentBy(Expression<Func<CertificateProjection,bool>> predicate);
    }
}