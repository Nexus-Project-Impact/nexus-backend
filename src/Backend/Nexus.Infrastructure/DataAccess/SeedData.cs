using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess
{
    public static class SeedData
    {
        public static async Task Initialize(
            NexusDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if(!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@nexus.com");

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin@nexus.com",
                    Name = "Administrador",
                    Email = "admin@nexus.com"
                    
                };

                var resultado = await userManager.CreateAsync(adminUser, "Senha@123");

                if(resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var user = await userManager.FindByEmailAsync("user@nexus.com");

            if (user == null)
            {
                user = new User
                {
                    UserName = "user@nexus.com",
                    Name = "Usuário",
                    Email = "user@nexus.com"
                };

                var resultado = await userManager.CreateAsync(user, "Senha@123");
                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }

            context.SaveChanges();
        }
    }
}
