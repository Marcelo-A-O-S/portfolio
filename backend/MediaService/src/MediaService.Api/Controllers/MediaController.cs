using MediaService.Application.DTOs.Requests;
using MediaService.Application.UseCases.MediaFiles.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MediaService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IAddMediaFile addMediaFile;
        private readonly IRemoveMediaFile removeMediaFile;
        public MediaController(
            IAddMediaFile _addMediaFile,
            IRemoveMediaFile _removeMediaFile
        )
        {
            this.addMediaFile = _addMediaFile;
            this.removeMediaFile = _removeMediaFile;
        }
        [HttpPost]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> CreateMedia([FromForm] MediaFileRequest mediaFileRequest)
        {
            if (ModelState.IsValid)
            {
                var mediaResponse = await this.addMediaFile.ExecuteAsync(mediaFileRequest);
                return Ok(mediaResponse);
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteMedia(Guid Id)
        {
            await this.removeMediaFile.ExecuteAsync(Id);
            return Ok();
        }
    }
}