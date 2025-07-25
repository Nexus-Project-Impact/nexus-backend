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

        [HttpPost("Register")]
        [Consumes("multipart/form-data")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterMidia(
            [FromServices] IMidiaUseCase midiaService,
            [FromForm] RequestMidia request)
        {
            var result = await midiaService.Execute(request);
            return Ok(result);
        }
    }
}
