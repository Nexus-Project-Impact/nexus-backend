using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Reservation.Create;
using Nexus.Application.UseCases.Reservation.Delete;
using Nexus.Application.UseCases.Reservation.GetAll;
using Nexus.Application.UseCases.Reservation.GetByID;
using Nexus.Application.UseCases.Reservation.GetBytravelerName;
using Nexus.Application.UseCases.Reservation.GetMyReservations;
using Nexus.Application.UseCases.Reservation.GetReservationByCpf;
using Nexus.Application.UseCases.Reservation.Update;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;

namespace Nexus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {

        private readonly IGetAllReservantionUseCase _getAllReservantionUseCase;
        private readonly IGetByIdReservationUseCase _getByIdReservationUseCase;
        private readonly IDeleteReservationUseCase _deleteReservationUseCase;
        private readonly IGetReservationByTravelerCpf _getReservationByTravelerCpf;
        private readonly IGetReservationByTravelerName _getReservationByTravelerName;
        private readonly IGetMyReservations _getMyReservations;
        private readonly IMapper _mapper;

        public ReservationController
        (
            IGetAllReservantionUseCase getAllReservantionUseCase,
            IGetByIdReservationUseCase getByIdReservationUseCase,
            IDeleteReservationUseCase deleteReservationUseCase,
            IGetReservationByTravelerCpf getReservationByTravelerCpf,
            IGetReservationByTravelerName getReservationByTravelerName,
            IGetMyReservations getMyReservations,
            IMapper mapper
        )
        {
            _deleteReservationUseCase = deleteReservationUseCase;
            _getAllReservantionUseCase = getAllReservantionUseCase;
            _getByIdReservationUseCase = getByIdReservationUseCase;
            _getReservationByTravelerCpf = getReservationByTravelerCpf;
            _getReservationByTravelerName = getReservationByTravelerName;
            _getMyReservations = getMyReservations;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(ResponseRegisteredReservationJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] ICreateReservationUseCase useCase, [FromBody] RequestRegisterReservationJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }

        [HttpGet("GetAll")]
        //[Authorize(Roles =("Admin"))]
        public async Task<ActionResult<IEnumerable<ResponseReservationJson>>> GetAll()
        {
            var reservations = await _getAllReservantionUseCase.ExecuteGetAllAsync();

            return Ok(reservations);
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservations = await _deleteReservationUseCase.ExecuteDeleteAsync(id);
            if (reservations == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("GetById/{id}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<ResponseReservationJson>> GetById(int id)
        {
            var reservation = await _getByIdReservationUseCase.ExecuteGetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            // Se a role for "User", filtra pelo userId
            if (User.IsInRole("User"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Forbid();

                if (reservation.UserId != userId)
                    return Forbid();
            }

            return Ok(reservation);
        }

        [HttpGet("GetReservationByTravelerName/{Name}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<ResponseReservationJson>>> GetReservationByTravelerName(string Name)
        {
            var reservations = await _getReservationByTravelerName.ExecuteGetReservationByTravelerNameAsync(Name);

            // Se a role for "User", filtra pelo userId
            if (User.IsInRole("User"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Forbid();

                reservations = reservations.Where(r => r.UserId == userId);
            }

            if (reservations == null || !reservations.Any())
                return NotFound();

            return Ok(reservations);
        }

        [HttpGet("GetReservationByTravelerCpf/{Cpf}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<ResponseReservationJson>>> GetReservationByTravelerCpf(string Cpf)
        {
            var reservations = await _getReservationByTravelerCpf.ExecuteGetReservationByTravelerCpfAsync(Cpf);

            if (User.IsInRole("User"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Forbid();

                reservations = reservations.Where(r => r.UserId == userId);
            }

            if (reservations == null || !reservations.Any())
            {
                return NotFound();
            }
            return Ok(reservations);
        }

        [HttpGet("MyReservations")]
        // [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ResponseReservationJson>>> GetMyReservations()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var reservations = await _getMyReservations.ExecuteGetMyReservationsAsync(userId);

            if (reservations == null || !reservations.Any())
                return NotFound();

            return Ok(reservations);
        }

        /*
        [HttpGet("GetById/{id}")]
        //[Authorize(Roles =("Admin, User"))]
        public async Task<ActionResult<ResponseReservationJson>> GetById(int id)
        {
            var reservations = await _getByIdReservationUseCase.ExecuteGetByIdAsync(id);

            if (reservations == null)
            {
                return NotFound();
            }
            return Ok(reservations);
        } 
         
        [HttpGet("GetReservationByTravelerName/{Name}")]
        //[Authorize("Admin")]

        public async Task<ActionResult<ResponseReservationJson>> GetReservationByTravelerName(string Name)
        {
            var reservations = await _getReservationByTravelerName.ExecuteGetReservationByTravelerNameAsync(Name);

            if (reservations == null)
            {
                return NotFound();
            }
            return Ok(reservations);
        }

        /*
        [HttpGet("GetReservationByTravelerCpf/{Cpf}")]
        //[Authorize("Admin")]
        public async Task<ActionResult<ResponseRegisteredReservationJson>> GetReservationByTravelerCpf(string Cpf)
        {
            var reservations = await _getReservationByTravelerCpf.ExecuteGetReservationByTravelerCpfAsync(Cpf);

            if (reservations == null)
            {
                return NotFound();
            }
            return Ok(reservations);
        }
        */
    }
}
