using PostService.Domain.Enums;
namespace PostService.Domain.Entities
{
    public class Post : PostBase
    {
        public ICollection<Tool> Tools { get; private set; }
        public ICollection<PostContent> PostContents { get; private set; }
        public Post(Status status)
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
            this.Categories = new List<Category>();
            this.Tools = new List<Tool>();
            this.PostContents = new List<PostContent>();
            this.ImgUrl = "";
            this.Status = status;
        }
        public void AddTool(Tool tool)
        {
            if (this.Tools == null)
                throw new Exception("Lista de ferramentas não inicializada.");
            this.Tools.Add(tool);
        }
        public void AddPostContent(PostContent postContent)
        {
            if (this.PostContents == null)
                throw new Exception("Lista de conteudo não inicializada.");
            this.PostContents.Add(postContent);
        }
        public void ValidateTools(IEnumerable<Guid> toolIds)
        {
            if(this.Tools == null)
                throw new Exception("Lista de ferramentas não inicializada.");
            var ids = toolIds.ToHashSet();
            var toRemove = this.Tools
                .Where(t=> !ids.Contains(t.Id))
                .ToList();
            foreach(var tool in toRemove)
                this.Tools.Remove(tool);
        }
        public void ValidatePostContents(IEnumerable<Guid> postContentIds)
        {
            if(this.PostContents == null)
                throw new Exception("Lista de conteúdo do post não inicializada.");
            var ids = postContentIds.ToHashSet();
            var toRemove = this.PostContents
                .Where(pt => !ids.Contains(pt.Id))
                .ToList();
            foreach(var postContent in toRemove)
                this.PostContents.Remove(postContent);
        }
    }
}