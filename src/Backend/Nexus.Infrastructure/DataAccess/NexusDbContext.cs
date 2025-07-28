using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess
{
    public class NexusDbContext : IdentityDbContext<User>
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
        {
        }

        public DbSet<Travelers> Travelers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Midia> Midias { get; set; }
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<TravelPackageEntity> TravelPackages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Midia>().ToTable("Midias");


            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Traveler)
                .WithOne(t => t.Reservation )
                .HasForeignKey("ReservationId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.TravelPackageEntity)
                .WithMany()
                .HasForeignKey(r => r.TravelPackageId)
                .OnDelete(DeleteBehavior.Restrict);

        }



    }
}
