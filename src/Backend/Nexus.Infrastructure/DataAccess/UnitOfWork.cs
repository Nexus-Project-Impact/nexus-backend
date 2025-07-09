using Nexus.Domain.Repositories;

namespace Nexus.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NexusDbContext _context;
        public UnitOfWork(NexusDbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
