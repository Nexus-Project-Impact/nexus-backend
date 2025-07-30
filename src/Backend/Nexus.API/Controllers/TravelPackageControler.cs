using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.UseCases.Packages.Create;
using Nexus.Application.UseCases.Packages.Delete;
using Nexus.Application.UseCases.Packages.GetAll;
using Nexus.Application.UseCases.Packages.GetByDepartureDate;
using Nexus.Application.UseCases.Packages.GetByDestination;
using Nexus.Application.UseCases.Packages.GetById;
using Nexus.Application.UseCases.Packages.GetByValue;
using Nexus.Application.UseCases.Packages.Update;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TravelPackageControler : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICreatePackageUseCase _createPackageUseCase;
        private readonly IGetByIdPackageUseCase _getByIdPackageUseCase;
        private readonly IGetAllPackageUseCase _getAllPackageUseCase;
        private readonly IUpdatePackageUseCase _updatePackageUseCase;
        private readonly IDeletePackageUseCase _deletePackageUseCase;
        private readonly IGetByDepartureDatePackageUseCase _getByDepartureDatePackageUseCase;
        private readonly IGetByDestinationPackageUseCase _getByDestinationPackageUseCase;
        private readonly IGetByValuePackageUseCase _getByByValuePackageUseCase;

        public TravelPackageControler(IMapper mapper, ICreatePackageUseCase createPackageUseCase, IGetByIdPackageUseCase getByIdPackageUseCase, IGetAllPackageUseCase getAllPackageUseCase, IUpdatePackageUseCase updatePackageUseCase, IDeletePackageUseCase deletePackageUseCase, IGetByDepartureDatePackageUseCase getByDepartureDatePackageUseCase, IGetByDestinationPackageUseCase getByDestinationPackageUseCase, IGetByValuePackageUseCase getByByValuePackageUseCase)
        {
            _mapper = mapper;
            _createPackageUseCase = createPackageUseCase;
            _getByIdPackageUseCase = getByIdPackageUseCase;
            _getAllPackageUseCase = getAllPackageUseCase;
            _updatePackageUseCase = updatePackageUseCase;
            _deletePackageUseCase = deletePackageUseCase;
            _getByDepartureDatePackageUseCase = getByDepartureDatePackageUseCase;
            _getByDestinationPackageUseCase = getByDestinationPackageUseCase;
            _getByByValuePackageUseCase = getByByValuePackageUseCase;
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponsePackage>> ExecuteGetById(int id)
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

        [HttpGet("GetAllPackages")]
        public async Task<ActionResult<IEnumerable<ResponsePackage>>> GetAll()
        {
            try
            {
                var packages = await _getAllPackageUseCase.ExecuteGetAll();

                if (packages == null || !packages.Any())
                    return NotFound();

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }




        [HttpPost("Create")]
        [ProducesResponseType(typeof(ResponseCreatedPackage), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromServices] ICreatePackageUseCase useCase, [FromBody] RequestCreatePackage request)
        {
            var result = await useCase.ExecuteCreate(request);
            return Created(string.Empty, result);
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(typeof(ResponsePackage), StatusCodes.Status201Created)]
        public async Task<IActionResult> Update([FromServices] IUpdatePackageUseCase useCase, int id, [FromBody] RequestUpdatePackage request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPackage = await _updatePackageUseCase.ExecuteUpdate(id, request);

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


                return Ok(new { message = "Pacote excluído com sucesso." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPost("GetByDepartureDate")]
        public async Task<ActionResult<IEnumerable<ResponsePackage>>> ExecuteGetByDepartureDate([FromBody] RequestDepartureDateRangePackage request)
        {
            if (request.InitialDate == default || request.FinalDate == default) 
                return BadRequest(new { message = "Os campos 'Data inicial' e 'Data final' são obrigatórios e devem estar em um formato válido." });

            if (request.InitialDate > request.FinalDate)
                return BadRequest(new { message = "A data inicial não pode ser maior que a data final." });

            try
            {
                var packages = await _getByDepartureDatePackageUseCase.ExecuteGetByDepartureDate(request.InitialDate, request.FinalDate);

                if (packages == null || !packages.Any())
                    return NotFound(new { message = $"Nenhum pacote encontrado entre {request.InitialDate:dd/MM/yyyy} e {request.FinalDate:dd/MM/yyyy}." });

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }

        }

        [HttpGet("GetByDestination")]
        public async Task<ActionResult<IEnumerable<ResponsePackage>>> ExecuteGetByDestination(string destination)
        {
            if (destination == default) return NotFound(new { message = $"O valor do campo Destino não pode ser nulo." });
            try
            {
                var packages = await _getByDestinationPackageUseCase.ExecuteGetByDestination(destination);  

                if (packages == null)
                    return NotFound(new { message = $"Pacote com destino {destination} não foi encontrado." });

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }

        }

        [HttpPost("GetByValue")]
        public async Task<ActionResult<IEnumerable<ResponsePackage>>> ExecuteGetByValue ([FromBody] RequestValueRangePackage request)
        {
            if (request.MinValue == 0 || request.MaxValue == 0)
                return BadRequest(new { message = "Os campos 'Valor minimo' e 'Valor máximo' são obrigatórios e devem estar em um formato válido." });

            if (request.MinValue > request.MaxValue)
                return BadRequest(new { message = "O valor minimo não pode ser maior que o valor máximo." });

            try
            {
                var packages = await _getByByValuePackageUseCase.ExecuteGetByValue(request.MinValue, request.MaxValue);

                if (packages == null || !packages.Any())
                    return NotFound(new { message = $"Nenhum pacote encontrado entre {request.MinValue: R$ 00,00} e {request.MaxValue:R$ 00,00}." });

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }

        }


    }
}
