using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Languages
{
    public class CreateLanguage : ICreateLanguage
    {
        private readonly ILanguageServices languageServices;
        public CreateLanguage(ILanguageServices _languageServices)
        {
            this.languageServices = _languageServices;
        }
        public async Task ExecuteAsync(LanguageRequest languageRequest)
        {
            ValidateLanguageRequest(languageRequest);
            await ValidateIfLanguageExists(languageRequest);
            var language = new Language(languageRequest.Code, languageRequest.Name);
            await this.languageServices.Save(language);
        }
        private static void ValidateLanguageRequest(LanguageRequest languageRequest)
        {
            var validationError = ValidationHelper.Validate(languageRequest);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
        private async Task ValidateIfLanguageExists(LanguageRequest languageRequest)
        {
            var language = await this.languageServices.FindBy(lg => lg.Code == languageRequest.Code && lg.Name == languageRequest.Name);
            if (language != null)
                throw new ValidationException("Dados inconsistentes.");
        }
    }
}