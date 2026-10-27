using System.Linq.Expressions;
using PostService.Domain.Entities;
namespace PostService.Application.Interfaces
{
    public interface ICertificateProjectionServices : IServices<CertificateProjection>
    {
        Task<CertificateProjection> FindWithContentBy(Expression<Func<CertificateProjection,bool>> predicate);
    }
}