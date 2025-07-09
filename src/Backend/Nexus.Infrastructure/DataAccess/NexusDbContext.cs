using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.DataAccess
{
    public class NexusDbContext : DbContext
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
        }

    }
}
