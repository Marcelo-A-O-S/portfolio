using PostService.Domain.Entities;
namespace PostService.Domain.Queries
{
    public class LinkView
    {
        public Guid? Id { get; set; }
        public string Url { get; set; }
        public ICollection<LinkDescriptionView> Descriptions { get; set; }
        public Guid LinkTypeId { get;  set; }
        public LinkTypeView LinkType { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ToolId { get; set; }

    }
}