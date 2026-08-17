namespace PostService.Application.Constants
{
    public class CacheKeys
    {
        public static string PostExists(Guid postId)
            => $"post:exists:{postId}";
        public static string ToolExists(Guid toolId)
            => $"tool:exists:{toolId}";
        public static string UserExists(Guid userId)
            => $"user:exists:{userId}";
        public static string ProviderExists(string providerId)
            => $"provider:exists:{providerId}";
        public static string LanguageExists(Guid languageId)
            => $"language:exists:{languageId}";
    }
}