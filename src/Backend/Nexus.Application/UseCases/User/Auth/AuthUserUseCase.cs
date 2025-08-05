using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Services.Auth;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Application.Services.Email;
using Microsoft.Extensions.Caching.Memory;

namespace Nexus.Application.UseCases.User.Auth
{
    public class AuthUserUseCase : IAuthUserUseCase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly SignInManager<Domain.Entities.User> _signInManager;

        public AuthUserUseCase(UserManager<Domain.Entities.User> userManager, IConfiguration config, 
            IJwtService jwtService, IMapper mapper, SignInManager<Domain.Entities.User> signIn, IEmailService emailService,
            IMemoryCache memoryCache)
        {
            _emailService = emailService;
            _userManager = userManager;
            _config = config;
            _jwtService = jwtService;
            _mapper = mapper;
            _signInManager = signIn;
            _memoryCache = memoryCache;
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
            var code = new Random().Next(100000, 999999).ToString();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            _memoryCache.Set($"pwdreset:{user.Email}:{code}", token, TimeSpan.FromMinutes(10));

            var subject = "Recuperação de Senha";
            var textMessage = $@"
            Seu código de recuperação é: {code}

            Não compartilhe este código com ninguém. Ele expira em 10 minutos.
            ";
            var htmlMessage = $@"
            <p>Seu código de recuperação é:</p>
            <p style='font-size:1.5em; font-weight:bold'>{code}</p>
            <p><strong>Nunca compartilhe este código com ninguém.</strong></p>
            <p>Ele expira em 10 minutos.</p>
            ";
            await _emailService.SendEmailAsync(user.Email, subject, textMessage, htmlMessage);

            return new ResponseForgotPassword
            {
                Success = true,
                Message = "E-mail enviado com sucesso."
            };
        }

        public async Task<bool> ResetPassword(RequestResetPassword request)
        {
            if (!_memoryCache.TryGetValue($"pwdreset:{request.email}:{request.code}", out string token))
                return false; 

            var user = await _userManager.FindByEmailAsync(request.email);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, token, request.newPassword);
            if (result.Succeeded)
            {
                _memoryCache.Remove($"pwdreset:{request.email}:{request.code}");
                return true;
            }
            return false;
        }
        public async Task Logout()
        {       
            await _signInManager.SignOutAsync();
        }
    }
}
