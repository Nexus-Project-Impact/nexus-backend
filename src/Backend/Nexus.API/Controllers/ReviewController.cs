using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Review;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        [HttpGet("GetAllReviews")]

        public async Task<ActionResult<IEnumerable<ResponseReviewJson>>> GetAllReviews([FromServices] IReviewUseCase useCase)
        {
            var reviews = await useCase.Execute();
            return Ok(reviews);
        }

        
    }
}
