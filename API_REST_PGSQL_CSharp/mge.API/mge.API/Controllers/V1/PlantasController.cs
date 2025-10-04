using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers.V1
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

        [HttpGet("{plantaId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid plantaId)
        {
            try
            {
                var unaPlanta = await _plantaService
                    .GetByIdAsync(plantaId);

                return Ok(unaPlanta);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Planta unaPlanta)
        {
            try
            {
                var plantaActualizada = await _plantaService
                    .UpdateAsync(unaPlanta);

                return Ok(plantaActualizada);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpDelete("{plantaId:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid plantaId)
        {
            try
            {
                var nombrePlantaBorrada = await _plantaService
                    .RemoveAsync(plantaId);

                return Ok($"La planta {nombrePlantaBorrada} fue eliminada correctamente!");
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}
