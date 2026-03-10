namespace AuthService.Domain.Entities
{
    public class PaginatedResult<T> where T : class
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}