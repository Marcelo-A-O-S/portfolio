using System.ComponentModel.DataAnnotations;
namespace backend_portifolio.Models
{
    public class Post
    {
        [Key]   
        public long Id { get; set; }
        public Guid Guid {get; set;}
        public List<Section> Sections {get; set;}

        public Post()
        {
            this.Sections = new List<Section>();
            this.Guid = Guid.NewGuid();
        }
    }
}