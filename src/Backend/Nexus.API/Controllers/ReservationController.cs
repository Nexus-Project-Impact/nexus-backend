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

        private readonly ICreateReservationUseCase _createReservationUseCase;
        private readonly IUpdateReservationUseCase _updateReservationUseCase;
        private readonly IGetAllReservantionUseCase _getAllReservantionUseCase;
        private readonly IGetByIdReservationUseCase _getByIdReservationUseCase;
        private readonly IDeleteReservationUseCase _deleteReservationUseCase;
        private readonly IMapper _mapper;

        public ReservationController
        (
            ICreateReservationUseCase createReservationUseCase, 
            IUpdateReservationUseCase updateReservationUseCase,
            IGetAllReservantionUseCase getAllReservantionUseCase, 
            IGetByIdReservationUseCase getByIdReservationUseCase,
            IDeleteReservationUseCase deleteReservationUseCase, 
            IMapper mapper
        )
        {
            _createReservationUseCase = createReservationUseCase;
            _deleteReservationUseCase = deleteReservationUseCase;
            _getAllReservantionUseCase = getAllReservantionUseCase;
            _getByIdReservationUseCase = getByIdReservationUseCase;
            _updateReservationUseCase = updateReservationUseCase;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin, Uset")]
        public async Task<IActionResult> Create(RequestReservation register)
        {
            var newPackage = _mapper.Map<Reservation>(register);

            await _createReservationUseCase.AddAsync(_mapper.Map<RequestReservation>(register));

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = newPackage.Id
                },
                _mapper.Map<ResponseReservation>(newPackage));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseReservation>>> GetAll()
        {
            var packages = await _getAllReservantionUseCase.GetAllAsync();

            return Ok(packages);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseReservation>> GetById(int id)
        {
            var packages = await _getByIdReservationUseCase.GetByIdAsync(id);

            if (packages == null)
            {
                return NotFound();
            }
            return Ok(packages);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateAsync(int id, RequestReservation register)
        {

            var packages = await _updateReservationUseCase.UpdateAsync(id, register);

            if (packages == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Delete(int id)
        {
            var packages = await _deleteReservationUseCase.DeleteAsync(id);
            if (packages == false)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
