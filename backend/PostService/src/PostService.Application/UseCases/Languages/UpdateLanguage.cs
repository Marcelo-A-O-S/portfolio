using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using PostService.Application.Exceptions;

namespace PostService.Application.UseCases.Languages
{
    public class UpdateLanguage : IUpdateLanguage
    {
        private readonly ILanguageServices languageServices;
        public UpdateLanguage(ILanguageServices _languageServices)
        {
            this.languageServices = _languageServices;
        }
        public async Task ExecuteAsync(Guid Id,LanguageRequest languageRequest)
        {
            ValidateLanguageRequest(languageRequest);
            var language = await GetById(Id);
            language.Update(languageRequest.Code, languageRequest.Name);
            await this.languageServices.Update(language);
        }
        private static void ValidateLanguageRequest(LanguageRequest languageRequest)
        {
            var validationError = ValidationHelper.Validate(languageRequest);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
        private async Task<Language> GetById(Guid Id)
        {
            var language = await this.languageServices.GetById(Id);
            if(language == null)
                throw new NotFoundException("Linguagem não encontrada.");
            return language;
        }
    }
}