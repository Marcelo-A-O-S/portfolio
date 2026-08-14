using static System.Runtime.InteropServices.JavaScript.JSType;
namespace PostService.Domain.Entities
{
    public class LinkType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string BackgroundColor { get; private set; }
        public string TextColor { get; private set; }
        public string Icon { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public LinkType(string name, string backgroundColor, string textColor, string icon)
        {
            this.Name = name;
            this.BackgroundColor = backgroundColor;
            this.TextColor = textColor;
            this.Icon = icon;
            this.CreatedAt = DateTime.UtcNow;
        }
        public void Update(string name, string backgroundColor, string textColor, string icon)
        {
            this.Name = name;
            this.BackgroundColor = backgroundColor;
            this.TextColor = textColor;
            this.Icon = icon;
        }
    }
}