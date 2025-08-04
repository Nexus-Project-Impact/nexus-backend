using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly NexusDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public ReviewRepository(NexusDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Domain.Entities.Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Domain.Entities.Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<IEnumerable<Domain.Entities.Review>> GetByPackageIdAsync(int packageId)
        {
            return await _context.Reviews
                .Where(r => r.PackageId == packageId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Domain.Entities.Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _unitOfWork.Commit();
        }

        public async Task UpdateAsync(Domain.Entities.Review review) // aqui representando o Moderate
        {
            _context.Reviews.Update(review);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
        }
    }
}
