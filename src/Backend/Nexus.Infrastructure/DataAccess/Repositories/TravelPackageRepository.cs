using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class TravelPackageRepository : IRepository<TravelPackage, int>
    {
        private readonly NexusDbContext _context;

        public TravelPackageRepository(NexusDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TravelPackage travelPackage)
        {
            await _context.TravelPackages.AddAsync(travelPackage);
        }
        public async Task<IEnumerable<TravelPackage>> GetAllAsync() => await _context.TravelPackages.ToListAsync();

        public async Task<TravelPackage?> GetByIdAsync(int id) => await _context.TravelPackages.FindAsync(id);


        public async Task UpdateAsync(TravelPackage travelPackage)
        {
            _context.TravelPackages.Update(travelPackage);
        }

        public async Task DeleteAsync(int id)
        {
            var travelPackage = await _context.TravelPackages.FindAsync(id);
            if (travelPackage != null)
            {
                _context.TravelPackages.Remove(travelPackage);
            }
        }


    }
}
