using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = ("Admin, User"))]
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
        //[Authorize(Roles = ("Admin, User"))]
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

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            var allClaims = User.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToList();
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            return Ok(new { 
                userId = userId,
                claims = allClaims,
                isAuthenticated = User.Identity?.IsAuthenticated,
                authType = User.Identity?.AuthenticationType
            });
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(ResponseRegisteredReviewJson), StatusCodes.Status201Created)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Register([FromServices] IRegisterReviewUseCase useCase, [FromBody] RequestRegisterReviewJson request)
        {
            // Extrair o UserId do token JWT
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { 
                    error = "UserId não encontrado no token",
                    message = "Usuário não autenticado corretamente"
                });
            }

            var result = await useCase.Execute(request, userId);
            return Created(string.Empty, result);
        }

        [HttpPut("Moderate/{id}")]
        [ProducesResponseType(typeof(ResponseModeratedReviewJson), StatusCodes.Status200OK)]
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
