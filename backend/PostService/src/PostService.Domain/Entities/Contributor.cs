namespace PostService.Domain.Entities
{
    public class Contributor
    {
        public Guid Id { get; private set; }
        public Guid? UserId { get; private set; }
        public Guid PostId { get; private set; }
        public Post Post { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string? ProfileUrl { get; private set;} 
        public Contributor(Guid postId, string name, string? description, string? profileUrl)
        {
            this.PostId = postId;
            this.Name = name;
            this.Description = description;
            this.ProfileUrl = profileUrl;
        }
        public void SetUserId(Guid userId)
        {
            this.UserId = userId;
        }
        public void Update(Guid postId, string name, string? description, string? profileUrl)
        {
            this.PostId = postId;
            this.Name = name;
            this.Description = description;
            this.ProfileUrl = profileUrl;
        }
    }
}