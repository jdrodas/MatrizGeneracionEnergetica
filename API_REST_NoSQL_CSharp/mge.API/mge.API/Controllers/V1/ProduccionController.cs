using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/produccion")]
    public class ProduccionController(ProduccionService produccionService) : Controller
    {
        private readonly ProduccionService _produccionService = produccionService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] ProduccionParametrosConsulta produccionParametrosConsulta)
        {
            try
            {
                var lasProducciones = await _produccionService
                    .GetAllAsync(produccionParametrosConsulta);

                return Ok(lasProducciones);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("planta/{plantaId:length(24)}")]

        public async Task<IActionResult> GetAllByPlantIdAsync(string plantaId)
        {
            try
            {
                var ProduccionAsociada = await _produccionService
                    .GetAllByPlantIdAsync(plantaId);

                return Ok(ProduccionAsociada);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("fecha/{fechaId:length(10)}")]
        public async Task<IActionResult> GetAllByDateIdAsync(string fechaId)
        {
            try
            {
                var ProduccionAsociada = await _produccionService
                    .GetAllByDateIdAsync(fechaId);

                return Ok(ProduccionAsociada);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{eventoId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string eventoId)
        {
            try
            {
                var unEvento = await _produccionService
                    .GetByIdAsync(eventoId);

                return Ok(unEvento);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Produccion unEvento)
        {
            try
            {
                var eventoCreado = await _produccionService
                    .CreateAsync(unEvento);

                return Ok(eventoCreado);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Produccion unEvento)
        {
            try
            {
                var eventoActualizado = await _produccionService
                    .UpdateAsync(unEvento);

                return Ok(eventoActualizado);
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

        [HttpDelete("{eventoId:length(24)}")]
        public async Task<IActionResult> RemoveAsync(string eventoId)
        {
            try
            {
                var eventoRemovido = await _produccionService
                    .RemoveAsync(eventoId);

                return Ok($"Se ha removido la producción de la planta {eventoRemovido.PlantaNombre} " +
                    $"para la fecha {eventoRemovido.Fecha} con un valor de {eventoRemovido.Valor} MW");
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
