using CertificateService.Application.DTOs.Responses;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CertificateService.Application.UseCases.Certificates
{
    public class AddPostProjectionCertificate : IAddPostProjectionCertificate
    {
        private readonly ICertificateServices certificateServices;
        private readonly IPostProjectionServices postProjectionServices;
        private readonly ILanguageProjectionServices languageProjectionServices;
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IPostServicesClient postServicesClient;
        private readonly IValidationServices validationServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IUnitOfWork unitOfWork;
        public AddPostProjectionCertificate(
            ICertificateServices _certificateServices,
            IPostProjectionServices _postProjectionServices,
            ILanguageProjectionServices _languageProjectionServices,
            IMediaProjectionServices _mediaProjectionServices,
            IPostServicesClient _postServicesClient,
            IValidationServices _validationServices,
            IRabbitMQProducer _rabbitMQProducer,
            IUnitOfWork _unitOfWork
        )
        {
            this.certificateServices = _certificateServices;
            this.postProjectionServices = _postProjectionServices;
            this.languageProjectionServices = _languageProjectionServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.postServicesClient = _postServicesClient;
            this.validationServices = _validationServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid certificateId, Guid postId)
        {
            await this.validationServices.ValidateCertificateExists(certificateId);
            var certificate = await this.certificateServices.GetById(certificateId);
            if (certificate == null)
                throw new NotFoundException("Certificado não encontrado.");
            await this.unitOfWork.BeginAsync();
            try
            {
                var postProjection = await this.postProjectionServices.GetById(postId);
                if (postProjection == null)
                {
                    var postResponse = await this.postServicesClient.GetPostAsync(postId);
                    if (postResponse == null)
                        throw new NotFoundException("Publicação não encontrada.");
                    postProjection = new PostProjection(postId, certificateId, postResponse.LikeCount, postResponse.CommentCount);
                    postProjection.GenerateId();
                    await ProcessPostContentProjections(postProjection, postResponse.PostContents);
                    await ProcessMediaProjection(postProjection, postResponse.Media);
                    await this.postProjectionServices.Save(postProjection);
                }
                certificate.AddPostProjection(postProjection);
                await this.certificateServices.Update(certificate);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await this.rabbitMQProducer.Publish("Certificate", new
            {
                CertificateId = certificateId,
                PostId = postId
            });
        }
        private async Task ProcessPostContentProjections(PostProjection postProjection, List<PostContentResponse> postContentResponses)
        {
            foreach (var item in postContentResponses)
            {
                var languageProjection = await this.languageProjectionServices
                        .FindBy(lp => lp.Code == item.Language.Code && lp.Name == item.Language.Name);
                if (languageProjection == null)
                {
                    languageProjection = new LanguageProjection(item.Language.Id, item.Language.Code, item.Language.Name);
                    languageProjection.GenerateId();
                    await this.languageProjectionServices.Save(languageProjection);
                }
                var postContentProjection = new PostContentProjection(item.Id, languageProjection.Id, item.Title, item.Description);
                postProjection.AddPostContentProjection(postContentProjection);
            }
        }
        private async Task ProcessMediaProjection(PostProjection postProjection, MediaResponse mediaResponse)
        {
            var mediaContent = await this.mediaProjectionServices.GetByUrl(mediaResponse.Url);
            if (mediaContent != null)
            {
                postProjection.SetThumbnail(mediaContent.Id);
                return;
            }
            mediaContent = new MediaProjection(mediaResponse.MediaId, mediaResponse.Url);
            mediaContent.GenerateId();
            await this.mediaProjectionServices.Save(mediaContent);
            postProjection.SetThumbnail(mediaContent.Id);
        }
    }
}