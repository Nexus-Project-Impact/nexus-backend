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

        public async Task ExecuteAddAsync(Travelers travelers)
        {
            await _context.Travelers.AddAsync(travelers);
        }

        public Task ExecuteDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Travelers>> ExecuteGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Travelers> ExecuteGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteUpdateAsync(Travelers entity)
        {
            throw new NotImplementedException();
        }
    }
}
