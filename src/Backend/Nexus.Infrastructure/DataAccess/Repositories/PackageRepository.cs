using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class PackageRepository : IRepository<TravelPackage, int>
    {
        private readonly NexusDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PackageRepository(NexusDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(TravelPackage travelPackage)
        {
            await _context.TravelPackages.AddAsync(travelPackage);

            await _unitOfWork.Commit();


        }
        public async Task<IEnumerable<TravelPackage>> GetAllAsync()
        {
            return await _context.TravelPackages.ToListAsync();
        }


        public async Task<TravelPackage?> GetByIdAsync(int id)
        {
            return await _context.TravelPackages.FindAsync(id);
        }



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
