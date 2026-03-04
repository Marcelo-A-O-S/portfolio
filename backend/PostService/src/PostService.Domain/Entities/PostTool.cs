namespace PostService.Domain.Entities
{
    public class PostTool
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set;}
        public Guid ToolId { get; set; }
        public Tool Tool { get; set;}
    }
}