using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/produccion")]
    public class ProduccionController(ProduccionService produccionService) : Controller
    {
        private readonly ProduccionService _produccionService = produccionService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasProducciones = await _produccionService
                .GetAllAsync();

            return Ok(lasProducciones);
        }

        [HttpGet("planta/{plantaId:Guid}")]
        public async Task<IActionResult> GetAllByPlantIdAsync(Guid plantaId)
        {
            try
            {
                var ProduccionAsociada = await _produccionService
                    .GetAllByPlantIdAsync(plantaId);

                return Ok(ProduccionAsociada);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
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
                return NotFound(error.Message);
            }
        }

        [HttpGet("{eventoId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid eventoId)
        {
            try
            {
                var unEvento = await _produccionService
                    .GetByIdAsync(eventoId);

                return Ok(unEvento);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
