using PostService.Domain.Entities;
namespace PostService.Domain.Queries
{
    public class LinkDescriptionView
    {
        public Guid Id { get;  set; }
        public string Title { get;  set; }
        public Guid LanguageId { get; set; }
        public LanguageView Language { get;  set; }
    }
}