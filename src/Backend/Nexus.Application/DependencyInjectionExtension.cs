using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Services.AutoMapper;
using Nexus.Application.Services.Cryptography;
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
        }
        private static void AddAutoMapper(IServiceCollection services)
        {
            // Correct the method call to use a configuration action
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>());
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
    }
}
