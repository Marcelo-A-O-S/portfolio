namespace PostService.Domain.Queries
{
    public class PostView : PostBaseView
    {
        public ICollection<ToolView> Tools { get; set; }
        public ICollection<PostContentView> PostContents { get; set; }
    }
}