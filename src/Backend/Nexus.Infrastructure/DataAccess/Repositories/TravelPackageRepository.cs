using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class TravelPackageRepository : IRepository<TravelPackageEntity, int>
    {
        private readonly NexusDbContext _context;

        public TravelPackageRepository(NexusDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TravelPackageEntity travelPackage)
        {
            await _context.TravelPackages.AddAsync(travelPackage);
        }
        public async Task<IEnumerable<TravelPackageEntity>> GetAllAsync()
        {
            return await _context.TravelPackages.ToListAsync();
        }
            

        public async Task<TravelPackageEntity?> GetByIdAsync(int id)
        {
           return await _context.TravelPackages.FindAsync(id);
        }
            


        public async Task UpdateAsync(TravelPackageEntity travelPackage)
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
