using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        [JsonIgnore]
        public ICollection<Tool> Tools {get; private set; }
        public ICollection<CategoryContent> CategoryContents { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public Category()
        {
            this.Id = Guid.NewGuid();
            this.Tools = new List<Tool>();
            this.CategoryContents = new List<CategoryContent>();
            this.CreatedAt =  DateTime.UtcNow;
        }
        public void AddCategoryContent(CategoryContent categoryContent)
        {
            if(this.CategoryContents == null)
                throw new Exception("Lista de conteudo da categoria não inicializada.");
            this.CategoryContents.Add(categoryContent);
        }
        public void ValidateCategoryContents(IEnumerable<Guid> categoryContentIds)
        {
            if(this.CategoryContents == null)
                throw new Exception("Lista de conteudo da categoria não inicializada.");
            var ids = categoryContentIds.ToHashSet();
            var toRemove = this.CategoryContents
                .Where(cc => !ids.Contains(cc.Id))
                .ToList();
            foreach(var categoryContent in toRemove)
                this.CategoryContents.Remove(categoryContent);
        }
    }
}