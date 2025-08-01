using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Dashboard;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("api/[controller]")]
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
    }
}
