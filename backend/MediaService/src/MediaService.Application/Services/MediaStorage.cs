using MediaService.Application.DTOs.Responses;
using MediaService.Application.Exceptions;
using MediaService.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MediaService.Application.Services
{
    public class MediaStorage : IMediaStorage
    {
        private readonly IWebHostEnvironment environment;
        public MediaStorage(
            IWebHostEnvironment _environment
        )
        {
            this.environment = _environment;
        }
        public Task DeleteAsync(string path)
        {
            var pathImage = Path.Combine(
                this.environment.WebRootPath,
                path
            );
            try
            {
                if (File.Exists(pathImage))
                {
                    File.Delete(pathImage);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar imagem.", ex);
            }
        }

        public Task DeleteTemporaryAsync(string temporaryPath)
        {
            try
            {
                if (File.Exists(temporaryPath))
                {
                    File.Delete(temporaryPath);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar imagem.", ex);
            }
        }

        public Task MoveToFinalLocationAsync(string temporaryPath, string finalPath)
        {
            var destinationPath = Path.Combine(
                this.environment.WebRootPath,
                finalPath
            );
            var destinationFolder = Path.GetDirectoryName(destinationPath);
            if (destinationFolder == null)
                throw new Exception("Diretório de destino inválido.");
            Directory.CreateDirectory(destinationFolder);
            File.Move(temporaryPath, destinationPath);
            return Task.CompletedTask;
        }
        public async Task<TemporaryMediaFile> SaveTemporaryAsync(IFormFile file, string folder)
        {
            if (file == null)
                throw new ValidationException("Arquivo não informado.");
            if (file.Length == 0)
                throw new ValidationException("Arquivo vazio.");
            var temporaryFolder = Path.Combine(
                this.environment.ContentRootPath,
                "media-temp"
            );
            if (!Directory.Exists(temporaryFolder))
                Directory.CreateDirectory(temporaryFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var temporaryPath = Path.Combine(temporaryFolder, fileName);
            var finalPath = Path.Combine("archives", folder, fileName).Replace("\\", "/");
            try
            {
                using var stream = new FileStream(temporaryPath, FileMode.Create);
                await file.CopyToAsync(stream);
                return new TemporaryMediaFile(
                    temporaryPath,
                    finalPath,
                    file.ContentType,
                    file.Length
                );
            }
            catch (Exception ex)
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
                throw new Exception("Erro ao salvar imagem.", ex);
            }
        }
    }
}