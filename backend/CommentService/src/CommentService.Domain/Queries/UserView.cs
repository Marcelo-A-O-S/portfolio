namespace CommentService.Domain.Queries
{
    public class UserView
    {
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
        public string Provider { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}