using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Services.Auth;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.User.Auth
{
    public class AuthUserUseCase : IAuthUserUseCase
    {
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
     //   private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly SignInManager<Domain.Entities.User> _signInManager;

        public AuthUserUseCase(UserManager<Domain.Entities.User> userManager, IConfiguration config, 
            JwtService jwtService, IMapper mapper, SignInManager<Domain.Entities.User> signIn)
        {
            _userManager = userManager;
            _config = config;
            _jwtService = jwtService;
            _mapper = mapper;
            _signInManager = signIn;
        }

        public async Task<ResponseLoginUserJson> Execute(RequestLoginUserJson request)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                // implementar para disparar erro específico para usuário não seja encontrado
                return new ResponseLoginUserJson
                {
                    Token = string.Empty,
                    Message = "User not found."
                };
            }

            if(user.Id == null || user.Email == null || user.Name == null)
            {
                // implementar para disparar erro específico para alguns campos nulos
                return new ResponseLoginUserJson
                {
                    Token = string.Empty,
                    Message = "Dados do usuário estão incompletos."
                };
            }
            var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!isPasswordValid.Succeeded)
            {
                return new ResponseLoginUserJson
                {
                    Token = string.Empty,
                    Message = "Invalid password."
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Name, roles);

            return new ResponseLoginUserJson
            {
                Token = token,
                Message = "Login bem sucedido!"
            };
        }

        public async Task<ResponseForgotPassword> Execute(RequestForgotPassword request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // implementar para disparar erro específico para usuário não seja encontrado
                return new ResponseForgotPassword
                {
                    Success = false,
                    Message = "Usuário não encontrado"
                };
            }

            // await _emailService.SendResetPasswordEmail(user);
            return new ResponseForgotPassword
            {
                Success = true,
                Message = "Usuário encontrado"
            };
        }
        
        public async Task Logout()
        {       
            await _signInManager.SignOutAsync();
        }
    }
}
