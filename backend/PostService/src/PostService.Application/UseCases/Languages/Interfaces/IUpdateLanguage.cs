using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Languages.Interfaces
{
    public interface IUpdateLanguage
    {
        Task ExecuteAsync(Guid Id,LanguageRequest languageRequest);
    }
}