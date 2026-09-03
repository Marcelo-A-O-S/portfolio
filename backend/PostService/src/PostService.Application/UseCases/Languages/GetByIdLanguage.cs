using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Domain.Queries;
namespace PostService.Application.UseCases.Languages
{
    public class GetByIdLanguage : IGetByIdLanguage
    {
        private readonly ILanguageServices languageServices;
        public GetByIdLanguage(
            ILanguageServices _languageServices
        )
        {
            this.languageServices = _languageServices;
        }
        public async Task<LanguageView> ExecuteAsync(Guid Id)
        {
            return await this.languageServices.GetByLanguageView(Id);
        }
    }
}