using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.TravelPackages.Create;
using Nexus.Application.UseCases.TravelPackages.Delete;
using Nexus.Application.UseCases.TravelPackages.GetAll;
using Nexus.Application.UseCases.TravelPackages.GetId;
using Nexus.Application.UseCases.TravelPackages.Update;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TravelPackageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGetAllPackageUseCase _allPackageUseCase;
        private readonly IGetByIdPackageUseCase _getByIdPackageUseCase;
        private readonly IUpdatePackageUseCase _updatePackageUseCase;
        private readonly IDeletePackageUseCase _deletePackageUseCase;
     

        public TravelPackageController(
            IMapper mapper, IGetAllPackageUseCase getAllPackageUseCase, IGetByIdPackageUseCase getByIdPackageUseCase, IUpdatePackageUseCase updatePackageUseCase, IDeletePackageUseCase deletePackageJsonUseCase)
        {
            _mapper = mapper;
            _allPackageUseCase = getAllPackageUseCase;
            _getByIdPackageUseCase = getByIdPackageUseCase;
            _updatePackageUseCase = updatePackageUseCase;
            _deletePackageUseCase = deletePackageJsonUseCase;
        }


        [HttpGet("GetAllPackages")]
        public async Task<ActionResult<IEnumerable<ResponsePackageJson>>> GetAll()
        {
            try
            {
                var packages = await _allPackageUseCase.ExecuteGetAll();

                if (packages == null || !packages.Any())
                    return NotFound();

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponsePackageJson>> ExecuteGetById(int id)
        {
            if (id == 0) return NotFound(new { message = $"O valor do campo Id não pode ser nulo." });
            try
            {
                var packages = await _getByIdPackageUseCase.ExecuteGetById(id);

                if (packages == null)
                    return NotFound(new { message = $"Pacote com ID {id} não foi encontrado." });

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }


        [HttpPost("Create")]
        [ProducesResponseType(typeof(ResponseRegisteredPackageJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] IRegisterPackageUseCase useCase, [FromBody] RequestRegisterPackageJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }


        [HttpPut("Update/{id}")]
        [ProducesResponseType(typeof(ResponseRegisteredPackageJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Update([FromServices] IRegisterPackageUseCase useCase, int id, [FromBody] RequestUpdatePackageJson request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPackage = await _updatePackageUseCase.ExecuteUpdate(id,request);

                if (updatedPackage == null)
                    return NotFound(new { message = $"Pacote com ID {id} não foi encontrado para atualização." });

                return Ok(updatedPackage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            try
            {
                var deleted = await _deletePackageUseCase.ExecuteDelete(id);

                if (!deleted)
                    return NotFound(new { message = $"Pacote com ID {id} não foi encontrado para exclusão." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }


    }
}

