using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class LanguageServices : ILanguageServices
    {
        private readonly ILanguageRepository languageRepository;
        public LanguageServices(ILanguageRepository _languageRepository)
        {
            this.languageRepository = _languageRepository;
        }
        public async Task Delete(Language entity)
        {
            await this.languageRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.languageRepository.DeleteById(Id);
        }

        public async Task<Language> FindBy(Expression<Func<Language, bool>> predicate)
        {
            return await this.languageRepository.FindBy(predicate);
        }

        public async Task<Language> GetById(Guid Id)
        {
            return await this.languageRepository.GetById(Id);
        }

        public async Task<PaginatedResult<Language>> GetPagination(int page, string? search, string? code)
        {
            return await this.languageRepository.GetPagination(page,search,code);
        }

        public async Task<List<Language>> List()
        {
            return await this.languageRepository.List();
        }

        public async Task<List<Language>> List(int page)
        {
            return await this.languageRepository.List(page);
        }

        public async Task Save(Language entity)
        {
            await this.languageRepository.Save(entity);
        }

        public async Task Update(Language entity)
        {
            await this.languageRepository.Update(entity);
        }
    }
}