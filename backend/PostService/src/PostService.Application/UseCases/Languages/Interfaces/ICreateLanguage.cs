using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Languages.Interfaces
{
    public interface ICreateLanguage
    {
        Task ExecuteAsync(LanguageRequest languageRequest);
    }
}