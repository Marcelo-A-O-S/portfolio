using System.ComponentModel.DataAnnotations;

namespace CertificateService.Domain.Entities
{
    public class PostProjection
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        public Guid CertificateId { get; private set; }
        public Certificate Certificate { get; private set; }
        public Guid MediaProjectionId { get; private set; }
        public MediaProjection MediaProjection { get; private set; }
        public ICollection<PostContentProjection> PostContents { get; private set; }
        public int LikeCount { get; private set; }
        public int CommentCount { get; private set; }
        public PostProjection(
            Guid postId,
            Guid certificateId,
            int likeCount,
            int commentCount
        )
        {
            this.PostId = postId;
            this.CertificateId = certificateId;
            this.PostContents = new List<PostContentProjection>();
            this.LikeCount = likeCount;
            this.CommentCount = commentCount;
        }
        public void GenerateId()
        {
            this.Id = Guid.NewGuid();
        }
        public void AddPostContentProjection(PostContentProjection postContentProjection)
        {
            if(this.PostContents == null)
                throw new ValidationException("Lista de conteudos não inicializada.");
            this.PostContents.Add(postContentProjection);
        }
        public void SetThumbnail(Guid mediaProjectionId)
        {
            this.MediaProjectionId = mediaProjectionId;
        }
    }
}