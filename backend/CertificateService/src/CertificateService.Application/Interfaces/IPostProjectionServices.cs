using System.Linq.Expressions;
using CertificateService.Domain.Entities;
namespace CertificateService.Application.Interfaces
{
    public interface IPostProjectionServices : IServices<PostProjection>
    {
        Task<PostProjection> FindWithContentBy(Expression<Func<PostProjection,bool>> predicate);
    }
}