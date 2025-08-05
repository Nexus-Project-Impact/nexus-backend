using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Services.Auth;
using Nexus.Application.Services.Email;
using Nexus.Application.Services.Payments;
using Nexus.Application.UseCases.Dashboard;
using Nexus.Application.UseCases.Dashboard.Exports.Excel;
using Nexus.Application.UseCases.Dashboard.Exports.Pdf;
using Nexus.Application.UseCases.Midia;
using Nexus.Application.UseCases.Packages.Create;
using Nexus.Application.UseCases.Packages.Delete;
using Nexus.Application.UseCases.Packages.GetAll;
using Nexus.Application.UseCases.Packages.GetByDepartureDate;
using Nexus.Application.UseCases.Packages.GetByDestination;
using Nexus.Application.UseCases.Packages.GetById;
using Nexus.Application.UseCases.Packages.GetByValue;
using Nexus.Application.UseCases.Packages.Update;
using Nexus.Application.UseCases.Payments.Create;
using Nexus.Application.UseCases.Payments.Read;
using Nexus.Application.UseCases.Reservation.Create;
using Nexus.Application.UseCases.Reservation.Delete;
using Nexus.Application.UseCases.Reservation.GetAll;
using Nexus.Application.UseCases.Reservation.GetByID;
using Nexus.Application.UseCases.Reservation.GetBytravelerName;
using Nexus.Application.UseCases.Reservation.GetMyReservations;
using Nexus.Application.UseCases.Reservation.GetReservationByCpf;
using Nexus.Application.UseCases.Review.Delete;
using Nexus.Application.UseCases.Review.GetAll;
using Nexus.Application.UseCases.Review.GetByPackageId;
using Nexus.Application.UseCases.Review.GetId;
using Nexus.Application.UseCases.Review.Moderate;
using Nexus.Application.UseCases.Review.Register;
using Nexus.Application.UseCases.TravelPackages.GetAllActive;
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
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IDeleteReviewUseCase, DeleteReviewUseCase>();
            services.AddScoped<IModerateReviewUseCase, ModerateReviewUseCase>();
            services.AddScoped<IRegisterReviewUseCase, RegisterReviewUseCase>();
            services.AddScoped<IGetByIdReviewUseCase, GetByIdReviewUseCase>();
            services.AddScoped<IGetAllReviewUseCase, GetAllReviewUseCase>();
            services.AddScoped<IGetByPackageIdReviewUseCase, GetByPackageIdReviewUseCase>();

            services.AddScoped<ICreatePackageUseCase, CreatePackageUseCase>();
            services.AddScoped<IGetAllPackageUseCase, GetAllPackageUseCase>();
            services.AddScoped<IGetByIdPackageUseCase, GetByIdPackageUseCase>();
            services.AddScoped<IUpdatePackageUseCase, UpdatePackageUseCase>();
            services.AddScoped<IDeletePackageUseCase, DeletePackageUseCase>();
            services.AddScoped<IGetByDepartureDatePackageUseCase, GetByDepartureDatePackageUseCase>();
            services.AddScoped<IGetByDestinationPackageUseCase, GetByDestinationPackageUseCase>();
            services.AddScoped<IGetByValuePackageUseCase, GetByValuePackageUseCase>();
            services.AddScoped<IGetAllActivePackageUseCase, GetAllActivePackageUseCase>();

            services.AddScoped<ICreateReservationUseCase, CreateReservationUseCase>();
            services.AddScoped<IGetAllReservantionUseCase, GetAllReservationUseCase>();
            services.AddScoped<IGetByIdReservationUseCase, GetByIdReservationUseCase>();
            services.AddScoped<IDeleteReservationUseCase, DeleteReservationUseCase>();

            services.AddScoped<ICreatePaymentUseCase, CreatePaymentUseCase>();
            services.AddScoped<IReadPaymentUseCase, ReadPaymentsUseCase>();

            services.AddScoped<IGetReservationByTravelerCpf, GetReservationByTravelerCpf>();
            services.AddScoped<IGetReservationByTravelerName, GetReservationByTravelerName>();
            services.AddScoped<IGetMyReservations, GetMyReservations>();

            services.AddScoped<IGetDashboardMetricsUseCase, GetDashboardMetricsUseCase>();
            services.AddScoped<IExportToExcelUseCase, ExportToExcelUseCase>();
            services.AddScoped<IExportToPdfUseCase, ExportToPdfUseCase>();

        }
        private static void AddJwtService(IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            services.AddScoped<IJwtService>(provider => new JwtService(secretKey, issuer, audience));
        }   

    }
}
