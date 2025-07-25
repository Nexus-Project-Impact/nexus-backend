using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Services.Auth;
using Nexus.Application.Services.Cryptography;
using Nexus.Application.UseCases.TravelPackages;
using Nexus.Application.UseCases.TravelPackages.Create;
using Nexus.Application.UseCases.TravelPackages.Delete;
using Nexus.Application.UseCases.TravelPackages.GetAll;
using Nexus.Application.UseCases.TravelPackages.GetId;
using Nexus.Application.UseCases.TravelPackages.Update;
using Nexus.Application.Services.Email;
using Nexus.Application.UseCases.Midia;
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
