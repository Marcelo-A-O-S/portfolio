using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Languages
{
    public class DeleteLanguage : IDeleteLanguage
    {
        private readonly ILanguageServices languageServices;
        public DeleteLanguage(ILanguageServices _languageServices)
        {
            this.languageServices = _languageServices;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var language = await GetLanguageById(Id);
            await this.languageServices.Delete(language);
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