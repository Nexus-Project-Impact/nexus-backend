using Nexus.Communication.Responses;
using Nexus.Domain.Repositories.Dashboard;

namespace Nexus.Application.UseCases.Dashboard;

public class GetDashboardMetricsUseCase : IGetDashboardMetricsUseCase
{
    private readonly IDashboardMetricsRepositoy _dashboardMetricsRepository;
   
    public GetDashboardMetricsUseCase(IDashboardMetricsRepositoy dashboardMetricsRepository)
    {
        _dashboardMetricsRepository = dashboardMetricsRepository;
    }
    public async Task<ResponseDashboardMetricsJson> Execute(DateTime? startDate, DateTime? endDate)
    {
        var salesByDestination = await _dashboardMetricsRepository.GetSalesByDestinationAsync(startDate, endDate);
        var salesByPeriod = await _dashboardMetricsRepository.GetSalesByPeriodAsync(startDate, endDate);
        var salesByStatus = await _dashboardMetricsRepository.GetSalesByStatusAsync(startDate, endDate);
        var summary = await _dashboardMetricsRepository.GetSummaryAsync(startDate, endDate);

        return new ResponseDashboardMetricsJson
        {
            SalesByDestination = salesByDestination,
            SalesByPeriod = salesByPeriod,
            SalesByStatus = salesByStatus,
            Summary = summary
        };
    }
}
