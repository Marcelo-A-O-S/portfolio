using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class CertificateContentProjectionServices : ICertificateContentProjectionServices
    {
        private readonly ICertificateContentProjectionRepository certificateContentProjectionRepository;
        public CertificateContentProjectionServices(
            ICertificateContentProjectionRepository _certificateContentProjectionRepository
        )
        {
            this.certificateContentProjectionRepository = _certificateContentProjectionRepository;
        }
        public async Task Delete(CertificateContentProjection entity)
        {
            await this.certificateContentProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.certificateContentProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.certificateContentProjectionRepository.Exists(Id);
        }

        public async Task<CertificateContentProjection> FindBy(Expression<Func<CertificateContentProjection, bool>> predicate)
        {
            return await this.certificateContentProjectionRepository.FindBy(predicate);
        }

        public async Task<CertificateContentProjection> GetById(Guid Id)
        {
            return await this.certificateContentProjectionRepository.GetById(Id);
        }

        public async Task<List<CertificateContentProjection>> List()
        {
            return await this.certificateContentProjectionRepository.List();
        }

        public async Task<List<CertificateContentProjection>> List(int page)
        {
            return await this.certificateContentProjectionRepository.List(page);
        }

        public async Task Save(CertificateContentProjection entity)
        {
            await this.certificateContentProjectionRepository.Save(entity);
        }

        public async Task Update(CertificateContentProjection entity)
        {
            await this.certificateContentProjectionRepository.Update(entity);
        }
    }
}