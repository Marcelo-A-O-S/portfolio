using PostService.Application.Caching.Interfaces;
using PostService.Application.Constants;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Languages
{
    public class DeleteLanguage : IDeleteLanguage
    {
        private readonly ILanguageServices languageServices;
        private readonly ILanguageCacheServices languageCacheServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteLanguage(
            ILanguageServices _languageServices,
            ILanguageCacheServices _languageCacheServices,
            IUnitOfWork _unitOfWork)
        {
            this.languageServices = _languageServices;
            this.languageCacheServices = _languageCacheServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var language = await GetLanguageById(Id);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.languageServices.Delete(language);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await this.languageCacheServices.RemoveLanguageCache(CacheKeys.LanguageExists(Id));
        }

        private async Task<Language> GetLanguageById(Guid Id)
        {
            var language = await this.languageServices.GetById(Id);
            if(language == null)
                throw new NotFoundException("Linguagem não encontrada.");
            return language;
        }
    }
}