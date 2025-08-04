using Nexus.Domain.DTOs;

namespace Nexus.Domain.Repositories.Dashboard
{
    public interface IDashboardMetricsRepositoy
    {
        public Task<IEnumerable<SalesByDestinationDto>> GetSalesByDestinationAsync(DateTime? startDate, DateTime? endDate);
        public Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync(DateTime? startDate, DateTime? endDate);
        //public Task<IEnumerable<SalesByStatusDto>> GetSalesByStatusAsync(DateTime? startDate, DateTime? endDate);
        public Task<DashboardSummaryDto> GetSummaryAsync(DateTime? startDate, DateTime? endDate);
    }
}
