namespace AuthService.Domain.Queries
{
    public class UserView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ProfileUrl { get; set; }
    }
}