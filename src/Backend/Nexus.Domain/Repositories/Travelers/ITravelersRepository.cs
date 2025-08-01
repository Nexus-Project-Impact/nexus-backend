using Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Repositories.Travelers
{
    public interface ITravelersRepository
    {
        Task AddAsync(Entities.Travelers entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Entities.Travelers>> GetAllAsync();
        Task<Entities.Travelers> GetByIdAsync(int id);
        Task UpdateAsync(Entities.Travelers entity);
    }
}
