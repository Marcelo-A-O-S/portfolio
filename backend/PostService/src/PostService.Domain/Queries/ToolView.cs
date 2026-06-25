namespace PostService.Domain.Queries
{
    public class ToolView : PostBaseView
    {
        public ICollection<ToolContentView> ToolContents { get; set; }
    }
}