using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Dashboard;
using Nexus.Application.UseCases.Dashboard.Exports.Excel;
using Nexus.Application.UseCases.Dashboard.Exports.Pdf;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        [HttpGet("metrics")]
        public async Task<ActionResult<ResponseDashboardMetricsJson>> GetMetrics(
            [FromServices] IGetDashboardMetricsUseCase useCase,
            [FromQuery]DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var result = await useCase.Execute(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel(
            [FromServices] IExportToExcelUseCase useCase,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var result = await useCase.Execute(startDate, endDate);
            return File(result.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Relatorio.xls");

        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf(
        [FromServices] IExportToPdfUseCase useCase,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
        {
            var result = await useCase.Execute(startDate, endDate);
            return File(result, "application/pdf", "Relatorio.pdf");
        }
    }
}
