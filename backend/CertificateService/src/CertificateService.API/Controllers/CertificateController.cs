using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CertificateService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly IAddCertificate addCertificate;
        private readonly IUpdateCertificate updateCertificate;
        private readonly IRemoveCertificate removeCertificate;
        public CertificateController(
            IAddCertificate _addCertificate,
            IRemoveCertificate _removeCertificate,
            IUpdateCertificate _updateCertificate
        )
        {
            this.addCertificate = _addCertificate;
            this.removeCertificate = _removeCertificate;
            this.updateCertificate = _updateCertificate;
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateCertificate([FromForm] CertificateRequest certificateRequest)
        {
            if (ModelState.IsValid)
            {
                await this.addCertificate.ExecuteAsync(certificateRequest);
                return Ok();
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateCertificate([FromRoute] Guid Id, [FromForm] CertificateRequest certificateRequest)
        {
            if (ModelState.IsValid)
            {
                await this.updateCertificate.ExecuteAsync(Id, certificateRequest);
                return Ok();
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteCertificate([FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                await this.removeCertificate.ExecuteAsync(Id);
                return Ok();
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
    }
}