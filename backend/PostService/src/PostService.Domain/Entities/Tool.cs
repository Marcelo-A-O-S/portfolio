using System.Text.Json.Serialization;
using PostService.Domain.Enums;
namespace PostService.Domain.Entities
{
    public class Tool : PostBase
    {
        [JsonIgnore]
        public ICollection<Post> Posts { get; private set; }
        public ICollection<ToolContent> ToolContents { get; private set;} 
        public Tool(Status status)
        {
            this.CreatedAt = DateTime.UtcNow;
            this.Posts = new List<Post>();
            this.ToolContents = new List<ToolContent>();
            this.Categories = new List<Category>();
            this.Status = status;
        }
        public void AddToolContent(ToolContent toolContent)
        {
            if(this.ToolContents == null)
                throw new Exception("Lista de conteudo não inicializada.");
            this.ToolContents.Add(toolContent);
        }
        public void ValidateToolContents(IEnumerable<Guid> toolContentIds)
        {
            if(this.ToolContents == null)
                throw new Exception("Lista de conteudo não inicializada.");
            var ids = toolContentIds.ToHashSet();
            var toRemove = this.ToolContents
                .Where(tc => !ids.Contains(tc.Id))
                .ToList();
            foreach(var toolContent in toRemove)
                this.ToolContents.Remove(toolContent);
        }
    }
}