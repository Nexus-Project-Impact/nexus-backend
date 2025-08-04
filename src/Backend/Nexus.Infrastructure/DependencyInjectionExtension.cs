using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Dashboard;
using Nexus.Infrastructure.DataAccess;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Nexus.Infrastructure.DataAccess.Repositories;

using Nexus.Infrastructure.DataAccess.Repositories;
using Nexus.Domain.Repositories.Reservation;
using Nexus.Domain.Repositories.Travelers;
using Nexus.Domain.Repositories.Packages;
using Nexus.Domain.Repositories.Review;
using System.Security.Claims;

namespace Nexus.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddIndentity(services);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            } 
           ).AddJwtBearer (options =>
            {
                var key = configuration["Jwt:Key"];
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.Zero,
                    // Mapear claims automaticamente
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                // Configurar eventos para debug (opcional)
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Log para debug - pode ser removido em produção
                        var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                        Console.WriteLine($"Token validado com claims: {string.Join(", ", claims ?? new string[0])}");
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NexusDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IPackageRepository<TravelPackage, int>, PackageRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ITravelersRepository, TravelersRepository>();

            services.AddScoped<IDashboardMetricsRepositoy, DashboardMetricsRepository>();

        }

        private static void AddIndentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<NexusDbContext>() 
                .AddDefaultTokenProviders();

        }
    }
}
