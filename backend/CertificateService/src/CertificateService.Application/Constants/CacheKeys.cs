namespace CertificateService.Application.Constants
{
    public class CacheKeys
    {
        public static string CertificateExists(Guid postId)
            => $"certificate:exists:{postId}";
    }
}