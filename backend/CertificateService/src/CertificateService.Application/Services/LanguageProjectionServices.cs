using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;

namespace CertificateService.Application.Services
{
    public class LanguageProjectionServices : ILanguageProjectionServices
    {
        private readonly ILanguageProjectionRepository languageProjectionRepository;
        public LanguageProjectionServices(
            ILanguageProjectionRepository _languageProjectionRepository
        )
        {
            this.languageProjectionRepository = _languageProjectionRepository;
        }
        public async Task Delete(LanguageProjection entity)
        {
            await this.languageProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.languageProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.languageProjectionRepository.Exists(Id);
        }

        public async Task<LanguageProjection> FindBy(Expression<Func<LanguageProjection, bool>> predicate)
        {
            return await this.languageProjectionRepository.FindBy(predicate);
        }

        public async Task<LanguageProjection> GetById(Guid Id)
        {
            return await this.languageProjectionRepository.GetById(Id);
        }

        public async Task<LanguageProjection> GetByLanguageId(Guid languageId)
        {
            return await this.languageProjectionRepository.GetByLanguageId(languageId);
        }

        public async Task<List<LanguageProjection>> List()
        {
            return await this.languageProjectionRepository.List();
        }

        public async Task<List<LanguageProjection>> List(int page)
        {
            return await this.languageProjectionRepository.List(page);
        }

        public async Task Save(LanguageProjection entity)
        {
            await this.languageProjectionRepository.Save(entity);
        }

        public async Task Update(LanguageProjection entity)
        {
            await this.languageProjectionRepository.Update(entity);
        }
    }
}