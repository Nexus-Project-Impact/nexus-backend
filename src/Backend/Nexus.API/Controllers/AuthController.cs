using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Review;
using Nexus.Application.UseCases.User.Auth;
using Nexus.Application.UseCases.User.Register;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase,[FromBody]RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }

        [HttpPost("test")]
        public async Task<IActionResult> Teste ([FromBody] RequestLoginUserJson request)
        {
            Console.WriteLine("Teste" + request.Email);
            return Ok(string.Empty);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseLoginUserJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromServices] IAuthUserUseCase useCase, [FromBody] RequestLoginUserJson request)
        {
            var result = await useCase.Execute(request);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseForgotPassword), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromServices] IAuthUserUseCase useCase, [FromBody] RequestForgotPassword request)
        {
            // simulação de envio de email
            // utilizar biblioteca de terceiros para envio de e-mail
            var result = await useCase.Execute(request);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseLoginUserJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout([FromServices] IAuthUserUseCase useCase)
            {
            await useCase.Logout();
            return Ok(new { message = "Logout realizado com sucesso!" });
        }

    }


}
