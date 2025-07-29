using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Nexus.Infrastructure.Configuration;
using Nexus.Infrastructure.Services;

namespace Nexus.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddIndentity(services);
            AddStripe(services, configuration);

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
                    ClockSkew = TimeSpan.Zero 
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
            services.AddScoped<IStripeService, Nexus.Infrastructure.Services.Stripe>();
        }

        private static void AddIndentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<NexusDbContext>() 
                .AddDefaultTokenProviders();
        }

        private static void AddStripe(IServiceCollection services, IConfiguration configuration)
        {
            var stripeSection = configuration.GetSection("Stripe");
            services.Configure<StripeSettings>(stripeSection);
            services.AddSingleton<IStripeSettings>(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<StripeSettings>>().Value);
        }
    }
}
