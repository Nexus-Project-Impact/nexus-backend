using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.User.Read;
using Nexus.Communication.Responses;


namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("getUserData")]
        [ProducesResponseType(typeof(ResponseUserData), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserData([FromServices] IReadUserUseCase useCase)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var response = await useCase.GetById(Guid.Parse(userId));

            return Ok(response);
        }

    }
}
