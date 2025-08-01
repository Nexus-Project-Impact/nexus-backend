namespace Nexus.Application.UseCases.Dashboard.Exports.Excel
{
    public interface IExportToExcelUseCase
    {
        Task<byte[]> Execute(DateTime? startDate, DateTime? endDate);
    }
}
