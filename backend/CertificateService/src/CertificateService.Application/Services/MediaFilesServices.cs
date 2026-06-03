using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CertificateService.Application.Services
{
    public class MediaFilesServices : IMediaFilesServices
    {
        private readonly IMediaFilesRepository mediaFilesRepository;
        private readonly IWebHostEnvironment environment;
        public MediaFilesServices(
            IMediaFilesRepository _mediaFilesRepository,
            IWebHostEnvironment _environment
        )
        {
            this.mediaFilesRepository = _mediaFilesRepository;
            this.environment = _environment;
        }
        public async Task Delete(MediaFile entity)
        {
            await this.mediaFilesRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.mediaFilesRepository.DeleteById(Id);
        }

        public async Task DeleteImageAsync(MediaFile mediaFile)
        {
            var pathImage = Path.Combine(
                this.environment.WebRootPath,
                mediaFile.Path
            );
            try
            {
                if (File.Exists(pathImage))
                {
                    File.Delete(pathImage);
                }
                await this.Delete(mediaFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar imagem.", ex);
            }
        }

        public async Task<MediaFile> FindBy(Expression<Func<MediaFile, bool>> predicate)
        {
            return await this.mediaFilesRepository.FindBy(predicate);
        }

        public async Task<MediaFile> GetById(Guid Id)
        {
            return await this.mediaFilesRepository.GetById(Id);
        }

        public async Task<MediaFile> GetByPath(string path)
        {
            return await this.mediaFilesRepository.GetByPath(path);
        }

        public async Task<List<MediaFile>> List()
        {
            return await this.mediaFilesRepository.List();
        }

        public async Task<List<MediaFile>> List(int page)
        {
            return await this.mediaFilesRepository.List(page);
        }

        public async Task Save(MediaFile entity)
        {
            await this.mediaFilesRepository.Save(entity);
        }

        public async Task<MediaFile?> SaveImageAsync(IFormFile file, string folder, bool isCommitted = false)
        {
            if (file == null || file.Length == 0)
                return null;
            var uploadsFolder = Path.Combine(
                this.environment.WebRootPath,
                folder
            );
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                var media = new MediaFile(
                    $"{folder}/{fileName}",
                    file.ContentType,
                    file.Length,
                    isCommitted
                );
                await this.mediaFilesRepository.Save(media);
                return media;
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                throw new Exception("Erro ao salvar imagem.", ex);
            }
        }

        public async Task Update(MediaFile entity)
        {
            await this.mediaFilesRepository.Update(entity);
        }
    }
}