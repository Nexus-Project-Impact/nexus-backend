using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Review;
using Nexus.Application.UseCases.Review.Delete;
using Nexus.Application.UseCases.Review.GetAll;
using Nexus.Application.UseCases.Review.GetId;
using Nexus.Application.UseCases.Review.Moderate;
using Nexus.Application.UseCases.Review.Register;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetAllReviewUseCase _allReviewUseCase;
        private readonly IGetByIdReviewUseCase _getByIdReviewUseCase;
        private readonly IModerateReviewUseCase _moderateReviewUseCase;
        private readonly IDeleteReviewUseCase _deleteReviewUseCase;

        public ReviewController(IMapper mapper, 
            IGetAllReviewUseCase allReviewUseCase, 
            IGetByIdReviewUseCase getByIdReviewUseCase, 
            IModerateReviewUseCase updateReviewUseCase, 
            IDeleteReviewUseCase deleteReviewUseCase)
        {
            _mapper = mapper;
            _allReviewUseCase = allReviewUseCase;
            _getByIdReviewUseCase = getByIdReviewUseCase;
            _moderateReviewUseCase = updateReviewUseCase;
            _deleteReviewUseCase = deleteReviewUseCase;
        }

        [HttpGet("GetAllReviews")]
        // TODO: ADICIONAR AUTORIZAÇÃO
        public async Task<ActionResult<IEnumerable<ResponseReviewJson>>> GetAll()
        {
            try
            {
                var reviews = await _allReviewUseCase.ExecuteGetAll();

                if (reviews == null || !reviews.Any())
                    return NotFound();

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpGet("GetById/{id}")]
        // TODO: ADICIONAR AUTORIZAÇÃO
        public async Task<ActionResult<ResponseReviewJson>> ExecuteGetById(int id)
        {
            if (id == 0) return NotFound(new { message = $"O valor do campo Id não pode ser nulo." });
            try
            {
                var reviews = await _getByIdReviewUseCase.ExecuteGetById(id);

                if (reviews == null)
                    return NotFound(new { message = $"Avaliação com ID {id} não foi encontrado." });

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(ResponseRegisteredReviewJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] IRegisterReviewUseCase useCase, [FromBody] RequestRegisterReviewJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }

        [HttpPut("Moderate/{id}")]
        [ProducesResponseType(typeof(ResponseModeratedReviewJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Moderate([FromServices] IModerateReviewUseCase moderateReviewUseCase,int id,[FromBody] RequestModerateReviewJson request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.ReviewId = id;

            try
            {
                var result = await moderateReviewUseCase.Execute(request);

                if (result == null)
                    return NotFound(new { message = $"Avaliação com ID {id} não foi encontrada para moderação." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var deleted = await _deleteReviewUseCase.ExecuteDelete(id);

                if (!deleted)
                    return NotFound(new { message = $"Avaliação com ID {id} não foi encontrado para exclusão." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }



    }
}
