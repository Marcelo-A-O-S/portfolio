namespace PostService.Domain.Queries
{
    public class ContributorView
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid PostId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ProfileUrl { get; set;} 
    }
}