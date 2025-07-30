using Nexus.Domain.DTOs;

namespace Nexus.Communication.Responses;

public class ResponseDashboardMetricsJson
{

    public IEnumerable<SalesByDestinationDto> SalesByDestination { get; set; }
    public IEnumerable<SalesByPeriodDto> SalesByPeriod { get; set; }
    public IEnumerable<SalesByStatusDto> SalesByStatus { get; set; }
    public DashboardSummaryDto Summary { get; set; }

}
