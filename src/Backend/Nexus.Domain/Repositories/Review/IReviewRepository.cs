using Nexus.Domain.Entities;

namespace Nexus.Domain.Repositories.Review
{
    public interface IReviewRepository : IRepository<Entities.Review, int>
    {
        Task<IEnumerable<Entities.Review>> GetByPackageIdAsync(int packageId);
    }
}