using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Repositories.Reservation
{
    public interface IReservationRepository
    {
        Task AddAsync(Entities.Reservation entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Entities.Reservation>> GetAllAsync();
        Task<Entities.Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Reservation>> GetReservationByTravelerNameAsync(string travelerName);
        Task<IEnumerable<Entities.Reservation>> GetReservationByCpfAsync(string travelerCpf);
    }
}
