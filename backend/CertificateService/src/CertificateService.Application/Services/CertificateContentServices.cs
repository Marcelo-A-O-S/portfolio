using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
namespace CertificateService.Application.Services
{
    public class CertificateContentServices : ICertificateContentServices
    {
        private readonly ICertificateContentRepository certificateContentRepository;
        public CertificateContentServices(
            ICertificateContentRepository _certificateContentRepository
        )
        {
            this.certificateContentRepository = _certificateContentRepository;
        }
        public async Task Delete(CertificateContent entity)
        {
            await this.certificateContentRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.certificateContentRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.certificateContentRepository.Exists(Id);
        }

        public async Task<CertificateContent> FindBy(Expression<Func<CertificateContent, bool>> predicate)
        {
            return await this.certificateContentRepository.FindBy(predicate);
        }

        public async Task<CertificateContent> GetById(Guid Id)
        {
            return await this.certificateContentRepository.GetById(Id);
        }

        public async Task<List<CertificateContent>> List()
        {
            return await this.certificateContentRepository.List();
        }

        public async Task<List<CertificateContent>> List(int page)
        {
            return await this.certificateContentRepository.List();
        }

        public async Task Save(CertificateContent entity)
        {
            await this.certificateContentRepository.Save(entity);
        }

        public async Task Update(CertificateContent entity)
        {
            await this.certificateContentRepository.Update(entity);
        }
    }
}