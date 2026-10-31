using System.Linq.Expressions;
using CertificateService.Domain.Entities;

namespace CertificateService.Domain.Interfaces
{
    public interface IPostProjectionRepository : IGenerics<PostProjection>
    {
        Task<PostProjection> FindWithContentBy(Expression<Func<PostProjection,bool>> predicate);
    }
}