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
             return await _context.Reservations
                .Include(r => r.Traveler)
                .Include(r => r.TravelPackage)
                .Include(r => r.Payment)
                .Include(r => r.User)
                .ToListAsync(); 
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(t => t.Traveler)
                .Include(r => r.TravelPackage)
                .Include(r => r.Payment)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
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
                .Include(r => r.TravelPackage)
                .Include(r => r.Payment)
                .Include(r => r.User)
                .Where(r => r.Traveler.Any(t => t.Name != null && t.Name.Contains(travelerName)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationByCpfAsync(string travelerCpf)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.TravelPackage)
                .Where(r => r.User != null && r.User.CPF != null && r.User.CPF.Contains(travelerCpf))
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetMyReservationsAsync(string userId)
        {
            return await _context.Reservations
                 .Include(r => r.User)
                 .Include(r => r.Traveler)
                 .Include(r => r.TravelPackage)
                 .Include(r => r.Payment)
                 .Where(r => r.UserId == userId)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> SearchByTravelerAsync(string? travelerName, string? travelerCpf)
        {
            return await _context.Reservations
                .Include(r => r.Traveler)
                .Include(r => r.TravelPackage)
                .Include(r => r.Payment)
                .Include(r => r.User)
                .Where(r =>
                    (string.IsNullOrWhiteSpace(travelerName) || r.Traveler.Any(t => t.Name != null && t.Name.Contains(travelerName))) &&
                    (string.IsNullOrWhiteSpace(travelerCpf) || (r.User != null && r.User.CPF != null && r.User.CPF.Contains(travelerCpf))))
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> SearchByUserAsync(string? userName, string? userCpf)
        {
            var query = _context.Reservations
                .Include(r => r.Traveler)
                .Include(r => r.TravelPackage)
                .Include(r => r.Payment)
                .Include(r => r.User)
                .AsQueryable();

            // Se nenhum parâmetro for fornecido, retorna todas as reservas
            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(userCpf))
            {
                return await query.ToListAsync();
            }

            // Aplicar filtros apenas se os parâmetros tiverem valor
            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(r => r.User!.Name!.Contains(userName));
            }

            if (!string.IsNullOrWhiteSpace(userCpf))
            {
                query = query.Where(r => r.User!.CPF!.Contains(userCpf));
            }

            return await query.ToListAsync();
        }
    }
}

