using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class ReservationRepository : IRepository<Reservation,int>
    {
        private readonly NexusDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationRepository(NexusDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync() 
        { 
             return await _context.Reservations.Include
                (r => r.Traveler).ToListAsync(); 
 
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations.Include(t => t.Traveler)
            .FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);

            await _unitOfWork.Commit();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);

            await _unitOfWork.Commit();
            /*
            var existingTravelers = await _context.Travelers
                        .Where(t => t.ReservationId == reservation.Id)
                        .ToListAsync();

            foreach (var traveler in reservation.Traveler)
            {
                if (traveler.Id == 0)
                {
                    _context.Travelers.Add(traveler);
                }
                else
                {
                    _context.Travelers.Update(traveler);
                }
            }

            foreach (var existing in existingTravelers)
            {
                if (!reservation.Traveler.Any(t => t.Id == existing.Id))
                {
                    _context.Travelers.Remove(existing);
                }
            }
            */
        }
        public async Task DeleteAsync(int id)
        {
            
            var item = await _context.Reservations.FindAsync(id);

            if (item != null)
            {
                _context.Reservations.Remove(item);

                await _unitOfWork.Commit();
            }
        }
    }
}

