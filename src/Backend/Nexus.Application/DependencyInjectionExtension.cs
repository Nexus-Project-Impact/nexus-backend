using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Services.AutoMapper;
using Nexus.Application.Services.Cryptography;
using Nexus.Application.UseCases.TravelPackages;
using Nexus.Application.UseCases.TravelPackages.Create;
using Nexus.Application.UseCases.TravelPackages.Delete;
using Nexus.Application.UseCases.TravelPackages.GetAll;
using Nexus.Application.UseCases.TravelPackages.GetId;
using Nexus.Application.UseCases.TravelPackages.Update;
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDeletePackageUseCase, DeletePackageUseCase>();
            services.AddScoped<IGetAllPackageUseCase, GetAllPackageUseCase>();
            services.AddScoped<IGetByIdPackageUseCase, GetByIdPackageUseCase>();
            services.AddScoped<IRegisterPackageUseCase, RegisterPackageUseCase>();
            services.AddScoped<IUpdatePackageUseCase, UpdatePackageUseCase>();
        }

        private static void AddPaswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
            services.AddScoped(option => new PasswordEncripter(additionalKey!));
        }
    }
}
