using Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Repositories.Review
{
    public interface IReviewRepository
    {

        Task<IEnumerable<Entities.Review>> GetAllAsync();

        Task<Entities.Review?> GetByIdAsync(string id);

        Task AddAsync(Entities.Review entity);

        Task DeleteAsync(string id);

    }
}
