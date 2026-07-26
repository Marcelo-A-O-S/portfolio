namespace AuthService.Application.DTOs.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string ProviderId { get; set; }
        public string Provider { get; set; }
    }
}