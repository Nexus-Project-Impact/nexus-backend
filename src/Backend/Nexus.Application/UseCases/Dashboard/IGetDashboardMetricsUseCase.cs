using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Dashboard;

public interface IGetDashboardMetricsUseCase
{
    public Task<ResponseDashboardMetricsJson> Execute(DateTime? startDate, DateTime? endDate);
}
