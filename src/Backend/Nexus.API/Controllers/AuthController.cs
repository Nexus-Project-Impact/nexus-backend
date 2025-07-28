using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
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
            var result = await useCase.Execute(request);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout([FromServices] IAuthUserUseCase useCase)
        {
            await useCase.Logout();
            return Ok(new { message = "Logout realizado com sucesso!" });
        }

        [HttpPost("reset-password")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword([FromServices] IAuthUserUseCase useCase, [FromBody] RequestResetPassword request)
        {
            var result = await useCase.ResetPassword(request);

            if (!result)
            {
                return BadRequest(new { message = "Erro ao redefinir a senha." });
            }
            return Ok(new ResponseMessage
            {
                Message = "Senha foi redefinida com sucesso"
            });
        }
    }
}
