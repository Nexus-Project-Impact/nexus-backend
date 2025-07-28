using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.TravelPackage;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TravelPackageController : ControllerBase
    {
        private readonly ITravelPackageUseCase _travelPackageUseCase;
        private readonly IMapper _mapper;

        public TravelPackageController(
            ITravelPackageUseCase travelPackageUseCase,
            IMapper mapper)
        {
            _travelPackageUseCase = travelPackageUseCase;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RequestTravelPackage register)
        {
            var newPackage = _mapper.Map<TravelPackageEntity>(register);

            await _travelPackageUseCase.AddAsync(_mapper.Map<RequestTravelPackage>(register));

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = newPackage.Id
                },
                _mapper.Map<ResponseTravelPackage>(newPackage));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseTravelPackage>>> GetAll()
        {
            var packages = await _travelPackageUseCase.GetAllAsync();

            return Ok(packages);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseTravelPackage>> GetById(int id)
        {
            var packages = await _travelPackageUseCase.GetByIdAsync(id);

            if (packages == null)
            {
                return NotFound();
            }
            return Ok(packages);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, RequestTravelPackage register)
        {
           
            var packages =  await _travelPackageUseCase.UpdateAsync(id, register);

            if (packages == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var packages = await _travelPackageUseCase.DeleteAsync(id);
            if (packages == false)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}

