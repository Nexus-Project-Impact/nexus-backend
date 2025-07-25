using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Repositories.Reservation
{
    public interface IReservationRepository
    {
        Task ExecuteAddAsync(Entities.Reservation entity);
        Task ExecuteDeleteAsync(int id);
        Task<IEnumerable<Entities.Reservation>> ExecuteGetAllAsync();
        Task<Entities.Reservation> ExecuteGetByIdAsync(int id);

    }
}
