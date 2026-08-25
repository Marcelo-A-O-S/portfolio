namespace CertificateService.Application.Constants
{
    public class CacheKeys
    {
        public static string CertificateExists(Guid certificateId)
            => $"certificate:exists:{certificateId}";
        public static string PostExists(Guid postId)
            => $"post:exists:{postId}";
    }
}