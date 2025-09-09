using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/produccion")]
    public class ProduccionController(ProducccionService produccionService) : Controller
    {
        private readonly ProducccionService _produccionService = produccionService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasProducciones = await _produccionService
                .GetAllAsync();

            return Ok(lasProducciones);
        }

        [HttpGet("planta/{planta_id:Guid}")]
        public async Task<IActionResult> GetAllByPlantIdAsync(Guid planta_id)
        {
            try
            {
                var ProduccionAsociada = await _produccionService
                    .GetAllByPlantIdAsync(planta_id);

                return Ok(ProduccionAsociada);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("fecha/{fecha_id:length(10)}")]
        public async Task<IActionResult> GetAllByDateIdAsync(string fecha_id)
        {
            try
            {
                var ProduccionAsociada = await _produccionService
                    .GetAllByDateIdAsync(fecha_id);

                return Ok(ProduccionAsociada);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{evento_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid evento_id)
        {
            try
            {
                var unEvento = await _produccionService
                    .GetByIdAsync(evento_id);

                return Ok(unEvento);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }


    }
}
