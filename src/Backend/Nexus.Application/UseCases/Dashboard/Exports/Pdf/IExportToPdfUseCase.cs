namespace Nexus.Application.UseCases.Dashboard.Exports.Pdf;

public interface IExportToPdfUseCase
{
    public Task<byte[]> Execute(DateTime? startDate, DateTime? endDate);
}
