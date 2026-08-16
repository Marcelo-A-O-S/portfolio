using PostService.Domain.Entities;
namespace PostService.Domain.Queries
{
    public class LinkView
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public Guid LinkTypeId { get;  set; }
        public LinkTypeView LinkType { get; set; }
        public Guid? PostId { get; private set; }
        public Guid? ToolId { get; private set; }

    }
}