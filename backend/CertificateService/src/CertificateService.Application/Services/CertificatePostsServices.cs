using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;

namespace CertificateService.Application.Services
{
    public class CertificatePostsServices : ICertificatePostsServices
    {
        private readonly ICertificatePostsRepository certificatePostsRepository;
        public CertificatePostsServices(
            ICertificatePostsRepository _certificatePostsRepository
        )
        {
            this.certificatePostsRepository = _certificatePostsRepository;
        }
        public async Task Delete(CertificatePost entity)
        {
            await this.certificatePostsRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.certificatePostsRepository.DeleteById(Id);
        }

        public async Task<CertificatePost> FindBy(Expression<Func<CertificatePost, bool>> predicate)
        {
            return await this.certificatePostsRepository.FindBy(predicate);
        }

        public async Task<CertificatePost> GetById(Guid Id)
        {
            return await this.certificatePostsRepository.GetById(Id);
        }

        public async Task<List<CertificatePost>> List()
        {
            return await this.certificatePostsRepository.List();
        }

        public async Task<List<CertificatePost>> List(int page)
        {
            return await this.certificatePostsRepository.List(page);
        }

        public async Task Save(CertificatePost entity)
        {
            await this.certificatePostsRepository.Save(entity);
        }

        public async Task Update(CertificatePost entity)
        {
            await this.certificatePostsRepository.Update(entity);
        }
    }
}