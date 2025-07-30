using Microsoft.EntityFrameworkCore;
using Nexus.Domain.DTOs;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Dashboard;

namespace Nexus.Infrastructure.DataAccess.Repositories;

public class DashboardMetricsRepository : IDashboardMetricsRepositoy
{
    private readonly NexusDbContext _context;
    public DashboardMetricsRepository(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SalesByDestinationDto>> GetSalesByDestinationAsync(DateTime? startDate, DateTime? endDate)
    {

        var query = _context.Reservations.AsQueryable();
        query = FilterByPeriod(query, startDate, endDate);

        

        var result = await query
            .GroupBy(r => r.TravelPackage!.Destination)
            .Select(g => new SalesByDestinationDto
            {
                Destination = g.Key,
                Quantity = g.Count()
            })
            .ToListAsync();

        return result;

    }

    public async Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync(DateTime? startDate, DateTime? endDate)
    {

        var query = _context.Reservations.AsQueryable();
        query = FilterByPeriod(query, startDate, endDate);

        var result = await query
            .GroupBy(r => r.ReservationDate.Date)
            .Select(g => new SalesByPeriodDto
            {
                Date = g.Key,
                Quantity = g.Count()
            })
            .OrderBy(r => r.Date)
            .ToListAsync();

        return result;

    }

    /*
    public async Task<IEnumerable<SalesByStatusDto>> GetSalesByStatusAsync(DateTime? startDate, DateTime? endDate)
    {

        var query = _context.Reservations.AsQueryable();
        query = FilterByPeriod(query, startDate, endDate);

        var total = await query.CountAsync();

        var result = await query
            .GroupBy(r => r.Status)
            .Select(g => new SalesByStatusDto
            {
                Status = g.Key,
                Quantity = g.Count()
            })
            .ToListAsync();

        return result;

    }
    */
    public async Task<DashboardSummaryDto> GetSummaryAsync(DateTime? startDate, DateTime? endDate)
    {

        var query = _context.Reservations.AsQueryable();
        query = FilterByPeriod(query, startDate, endDate);

        var totalReservations = await query.CountAsync();
        var totalClients = await query.Select(r => r.UserId).Distinct().CountAsync();

        var topDestinations = await query
            .GroupBy(r => r.TravelPackage!.Destination)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToListAsync();

        return new DashboardSummaryDto
        {
            TotalReservations = totalReservations,
            TotalClients = totalClients,
            TopDestinations = topDestinations
        };

    }

    private IQueryable<Reservation> FilterByPeriod(IQueryable<Reservation> query, DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(r => r.ReservationDate >= startDate.Value && r.ReservationDate <= endDate.Value);
        }
        
        return query;
    }
}
