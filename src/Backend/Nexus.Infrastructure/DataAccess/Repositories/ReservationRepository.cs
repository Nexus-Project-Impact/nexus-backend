using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly NexusDbContext _context;


        public ReservationRepository(NexusDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Reservation>> GetAllAsync() 
        { 
             return await _context.Reservations.Include
                (r => r.Traveler).ToListAsync(); 
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            var reservation = await _context.Reservations.Include(t => t.Traveler).FirstOrDefaultAsync(r => r.Id == id);
            return reservation!;
        }

        

        public async Task AddAsync(Reservation reservation)
        {
            reservation.ReservationNumber = await GetNextReservationNumberAsync();
            await _context.Reservations.AddAsync(reservation);

        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Reservations.FindAsync(id);

            if (item != null)
            {
                _context.Reservations.Remove(item);

            }
        }

        // Novo método para obter o próximo número de reserva
        private async Task<int> GetNextReservationNumberAsync()
        {
            var maxNumber = await _context.Reservations.MaxAsync(r => (int?)r.ReservationNumber) ?? 0;
            return maxNumber + 1;
        }

        public async Task<IEnumerable<Reservation>> GetReservationByTravelerNameAsync(string travelerName)
        {
            return await _context.Reservations
                .Include(r => r.Traveler)
                .Where(r => r.Traveler.Any(t => t.Name != null && t.Name.Contains(travelerName)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationByCpfAsync(string travelerCpf)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Where(r => r.User != null && r.User.CPF != null && r.User.CPF.Contains(travelerCpf))
                .ToListAsync();
        }
    }
}

