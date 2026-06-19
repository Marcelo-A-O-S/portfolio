using System.Linq.Expressions;
using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MediaService.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
namespace MediaService.Application.Services
{
    public class MediaServices : IMediaServices
    {
        private readonly IWebHostEnvironment environment;
        private readonly IMediaRepository repository;
        public MediaServices(IMediaRepository _repository, IWebHostEnvironment _environment)
        {
            this.repository = _repository;
            this.environment = _environment;
        }
        public async Task Delete(Media entity)
        {
            await this.repository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.repository.DeleteById(Id);
        }
        public async Task DeleteImageAsync(Media mediaFile)
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

        public async Task<bool> Exists(Guid Id)
        {
            return await this.repository.Exists(Id);
        }

        public async Task<Media> FindBy(Expression<Func<Media, bool>> predicate)
        {
            return await this.repository.FindBy(predicate);
        }

        public async Task<Media> GetById(Guid Id)
        {
            return await this.repository.GetById(Id);
        }

        public async Task<Media> GetByPath(string path)
        {
            return await this.repository.GetByPath(path);
        }

        public async Task<List<Media>> List()
        {
            return await this.repository.List();
        }

        public async Task<List<Media>> List(int page)
        {
            return await this.repository.List(page);
        }

        public async Task<List<Media>> ListExpiredUncommittedMediaAsync()
        {
            return await this.repository.ListExpiredUncommittedMediaAsync();
        }

        public async Task Save(Media entity)
        {
            await this.repository.Save(entity);
        }

        public async Task<Media> SaveImageAsync(Guid? ownerId, string ownerType, IFormFile file, string folder)
        {
            if (file == null)
                throw new ValidationException("Arquivo não informado.");
            if (file.Length == 0)
                throw new ValidationException("Arquivo vazio.");
            var uploadsFolder = Path.Combine(
                this.environment.WebRootPath,
                "archives",
                folder
            );
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            var relativePath = Path.Combine("archives",folder,fileName).Replace("\\", "/");
            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                var media = new Media(
                    ownerId,
                    ownerType,
                    relativePath,
                    file.ContentType,
                    file.Length
                );
                media.MarkAsUploaded();
                await this.repository.Save(media);
                return media;
            }catch(Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                throw new Exception("Erro ao salvar imagem.", ex);
            }
        }

        public async Task Update(Media entity)
        {
            await this.repository.Update(entity);
        }
    }
}