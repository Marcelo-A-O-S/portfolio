namespace PostService.Application.DTOs.Deserialize
{
    public class ToolDeserialize
    {
        public Guid? Id { get; set; }
        public List<ToolContentDeserialize> toolContents { get; set; }
    }
}