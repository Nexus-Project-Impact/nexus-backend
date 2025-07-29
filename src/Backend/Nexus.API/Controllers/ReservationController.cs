using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Reservation.Create;
using Nexus.Application.UseCases.Reservation.Delete;
using Nexus.Application.UseCases.Reservation.GetAll;
using Nexus.Application.UseCases.Reservation.GetByID;
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

        // private readonly IUpdateReservationUseCase _updateReservationUseCase;
        private readonly IGetAllReservantionUseCase _getAllReservantionUseCase;
        private readonly IGetByIdReservationUseCase _getByIdReservationUseCase;
        private readonly IDeleteReservationUseCase _deleteReservationUseCase;
        private readonly IMapper _mapper;

        public ReservationController
        (
            // IUpdateReservationUseCase updateReservationUseCase,
            IGetAllReservantionUseCase getAllReservantionUseCase, 
            IGetByIdReservationUseCase getByIdReservationUseCase,
            IDeleteReservationUseCase deleteReservationUseCase, 
            IMapper mapper
        )
        {
            // _updateReservationUseCase = updateReservationUseCase;
            _deleteReservationUseCase = deleteReservationUseCase;
            _getAllReservantionUseCase = getAllReservantionUseCase;
            _getByIdReservationUseCase = getByIdReservationUseCase;
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
        public async Task<ActionResult<IEnumerable<ResponseRegisteredReservationJson>>> GetAll()
        {
            var packages = await _getAllReservantionUseCase.ExecuteGetAllAsync();

            return Ok(packages);
        }

        [HttpGet("GetById/{id}")]
        //[Authorize(Roles =("Admin, User"))]
        public async Task<ActionResult<ResponseRegisteredReservationJson>> GetById(int id)
        {
            var packages = await _getByIdReservationUseCase.ExecuteGetByIdAsync(id);

            if (packages == null)
            {
                return NotFound();
            }
            return Ok(packages);
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var packages = await _deleteReservationUseCase.ExecuteDeleteAsync(id);
            if (packages == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        /*
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateAsync(int id, RequestRegisterReservationJson register)
        {

            var packages = await _updateReservationUseCase.ExecuteUpdateAsync(id, register);

            if (packages == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        */

    }
}
