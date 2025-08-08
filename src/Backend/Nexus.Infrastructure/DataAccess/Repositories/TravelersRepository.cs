using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Travelers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class TravelersRepository : ITravelersRepository
    {
        private readonly NexusDbContext _context;

        public TravelersRepository(NexusDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Travelers travelers)
        {
            await _context.Travelers.AddAsync(travelers);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Travelers>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Travelers> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Travelers entity)
        {
            throw new NotImplementedException();
        }
    }
}
