using PostService.Domain.Queries;

namespace PostService.Application.UseCases.Languages.Interfaces
{
    public interface IGetByIdLanguage
    {
        Task<LanguageView> ExecuteAsync(Guid Id);
    }
}