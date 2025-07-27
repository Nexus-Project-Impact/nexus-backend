using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Services.Auth;
using Nexus.Application.Services.Cryptography;
using Nexus.Application.Services.Email;
using Nexus.Application.UseCases.Midia;
using Nexus.Application.UseCases.Review.Delete;
using Nexus.Application.UseCases.Review.GetAll;
using Nexus.Application.UseCases.Review.GetId;
using Nexus.Application.UseCases.Review.Moderate;
using Nexus.Application.UseCases.Review.Register;
using Nexus.Application.UseCases.User.Auth;
using Nexus.Application.UseCases.User.Register;

namespace Nexus.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddUseCases(services);
            AddAutoMapper(services);
            AddPaswordEncrypter(services, configuration);
            AddJwtService(services, configuration); 
        }
        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IAuthUserUseCase, AuthUserUseCase>();
            services.AddScoped<IMidiaUseCase, MidiaUseCase>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDeleteReviewUseCase, DeleteReviewUseCase>();
            services.AddScoped<IDeleteReviewUseCase, DeleteReviewUseCase>();
            services.AddScoped<IModerateReviewUseCase, ModerateReviewUseCase>();
            services.AddScoped<IRegisterReviewUseCase, RegisterReviewUseCase>();
            services.AddScoped<IGetByIdReviewUseCase, GetByIdReviewUseCase>();
            services.AddScoped<IGetAllReviewUseCase, GetAllReviewUseCase>();
        }

        private static void AddPaswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
            services.AddScoped(option => new PasswordEncripter(additionalKey!));
        }
        private static void AddJwtService(IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            services.AddScoped<JwtService>(provider => new JwtService(secretKey, issuer, audience));
        }   

    }
}
