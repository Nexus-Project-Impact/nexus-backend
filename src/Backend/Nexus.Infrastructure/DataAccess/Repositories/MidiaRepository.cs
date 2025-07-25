using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;


namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class MidiaRepository : IRepository<Midia, string>
    {
        private readonly NexusDbContext _context;

        public MidiaRepository(NexusDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Midia midia)
        {
            await _context.Midias.AddAsync(midia);
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Midia>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Midia?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Midia entity)
        {
            throw new NotImplementedException();
        }
    }
}
