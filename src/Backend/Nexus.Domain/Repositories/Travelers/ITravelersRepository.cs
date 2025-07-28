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
        Task ExecuteAddAsync(Entities.Travelers entity);
        Task ExecuteDeleteAsync(int id);
        Task<IEnumerable<Entities.Travelers>> ExecuteGetAllAsync();
        Task<Entities.Travelers> ExecuteGetByIdAsync(int id);
        Task ExecuteUpdateAsync(Entities.Travelers entity);
    }
}
