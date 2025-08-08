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
            return await _context.Reviews
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Domain.Entities.Review>> GetByPackageIdAsync(int packageId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.PackageId == packageId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Domain.Entities.Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task UpdateAsync(Domain.Entities.Review review)
        {
            // Ensure the entity is being tracked and marked as modified
            var existingReview = await _context.Reviews.FindAsync(review.Id);
            if (existingReview != null)
            {
                // Update the properties
                existingReview.Comment = review.Comment;
                existingReview.Rating = review.Rating;
                existingReview.PackageId = review.PackageId;
                existingReview.UserId = review.UserId;
                
                // The context will automatically track changes
                _context.Reviews.Update(existingReview);
            }
            else
            {
                // If not found in context, use the provided review
                _context.Reviews.Update(review);
            }
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
