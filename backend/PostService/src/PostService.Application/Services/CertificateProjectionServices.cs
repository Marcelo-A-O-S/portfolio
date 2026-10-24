using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class CertificateProjectionServices : ICertificateProjectionServices
    {
        private readonly ICertificateProjectionRepository certificateProjectionRepository;
        public CertificateProjectionServices(
            ICertificateProjectionRepository _certificateProjectionRepository
        )
        {
            this.certificateProjectionRepository = _certificateProjectionRepository;
        }
        public async Task Delete(CertificateProjection entity)
        {
            await this.certificateProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.certificateProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.certificateProjectionRepository.Exists(Id);
        }

        public async Task<CertificateProjection> FindBy(Expression<Func<CertificateProjection, bool>> predicate)
        {
            return await this.certificateProjectionRepository.FindBy(predicate);
        }

        public async Task<CertificateProjection> GetById(Guid Id)
        {
            return await this.certificateProjectionRepository.GetById(Id);
        }

        public async Task<List<CertificateProjection>> List()
        {
            return await this.certificateProjectionRepository.List();
        }

        public async Task<List<CertificateProjection>> List(int page)
        {
            return await this.certificateProjectionRepository.List(page);
        }

        public async Task Save(CertificateProjection entity)
        {
            await this.certificateProjectionRepository.Save(entity);
        }

        public async Task Update(CertificateProjection entity)
        {
            await this.certificateProjectionRepository.Update(entity);
        }
    }
}