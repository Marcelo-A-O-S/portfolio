using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
namespace CertificateService.Application.Services
{
    public class CertificateServices : ICertificateServices
    {
        private readonly ICertificateRepository certificateRepository;
        public CertificateServices(ICertificateRepository _certificateRepository)
        {
            this.certificateRepository = _certificateRepository;
        }
        public async Task Delete(Certificate entity)
        {
            await this.certificateRepository.Delete(entity);
        }

        public async Task<Certificate> FindBy(Expression<Func<Certificate, bool>> predicate)
        {
            return await this.certificateRepository.FindBy(predicate);
        }

        public async Task<Certificate> GetById(Guid Id)
        {
            return await this.certificateRepository.GetById(Id);
        }

        public async Task<List<Certificate>> List()
        {
            return await this.certificateRepository.List();
        }

        public async Task<List<Certificate>> List(int page)
        {
            return await this.certificateRepository.List(page);
        }

        public async Task Save(Certificate entity)
        {
            await this.certificateRepository.Save(entity);
        }

        public async Task Update(Certificate entity)
        {
            await this.certificateRepository.Update(entity);
        }
    }
}