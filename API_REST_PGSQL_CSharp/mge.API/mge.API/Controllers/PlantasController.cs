using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/plantas")]
    public class PlantasController(PlantaService plantaService) : Controller
    {
        private readonly PlantaService _plantaService = plantaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasPlantas = await _plantaService
                .GetAllAsync();

            return Ok(lasPlantas);
        }

        [HttpGet("{planta_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid planta_id)
        {
            try
            {
                var unaPlanta = await _plantaService
                    .GetByIdAsync(planta_id);

                return Ok(unaPlanta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Planta unaPlanta)
        {
            try
            {
                var plantaCreada = await _plantaService
                    .CreateAsync(unaPlanta);

                return Ok(plantaCreada);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}
