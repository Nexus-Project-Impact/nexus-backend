using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.User.Register;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase,[FromBody]RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }
    }
}
