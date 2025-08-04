using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Midia;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MidiaController : ControllerBase
    {

        private string GetMimeType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();

            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        //[HttpPost("Register")]
        //[Consumes("multipart/form-data")]
        //[Authorize]
        //[ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        //public async Task<IActionResult> RegisterMidia(
        //    [FromServices] IMidiaUseCase midiaService,
        //    [FromForm] RequestMidia request)
        //{
        //    var result = await midiaService.Execute(request);
        //    return Ok(result);
        //}

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Nome da imagem é obrigatório.");

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", name);

            if (!System.IO.File.Exists(imagePath))
                return NotFound("Imagem não encontrada.");

            var mimeType = GetMimeType(name);

            return PhysicalFile(imagePath, mimeType);
        }
    }
}
