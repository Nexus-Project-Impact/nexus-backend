namespace Nexus.Domain.DTOs;

public class DashboardSummaryDto
{
    public int TotalReservations { get; set; }
    public int TotalClients { get; set; }
    public List<string>? TopDestinations { get; set; }

}
