namespace PostService.Domain.Queries
{
    public class PostView : PostBaseView
    {
        public ICollection<ContributorView> Contributors { get; set; }
        public ICollection<ToolView> Tools { get; set; }
        public ICollection<PostContentView> PostContents { get; set; }
    }
}