namespace MediaService.Application.Constants
{
    public static class CacheKeys
    {
        public static string PostExists(Guid postId)
            => $"post:exists:{postId}";
        public static string ToolExists(Guid toolId)
            => $"tool:exists:{toolId}";
        public static string CertificateExists(Guid certificateId)
            => $"certificate:exists:{certificateId}";
    }
}