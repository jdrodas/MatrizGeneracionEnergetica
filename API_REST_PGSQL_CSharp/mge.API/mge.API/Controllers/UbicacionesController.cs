using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/ubicaciones")]
    public class UbicacionesController(UbicacionService ubicacionService) : Controller
    {
        private readonly UbicacionService _ubicacionService = ubicacionService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasUbicaciones = await _ubicacionService
                .GetAllAsync();

            return Ok(lasUbicaciones);
        }

        [HttpGet("{ubicacion_id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid ubicacion_id)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                    .GetByIdAsync(ubicacion_id);

                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{ubicacion_nombre:maxlength(100)}")]
        public async Task<IActionResult> GetByNameAsync(string ubicacion_nombre)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                    .GetByNameAsync(ubicacion_nombre);

                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{depto_iso:length(6)}/plantas")]
        public async Task<IActionResult> GetAssociatedPlantsByDeptoIsoAsync(string depto_iso)
        {
            try
            {
                var lasPlantasAsociadas = await _ubicacionService
                    .GetAssociatedPlantsByDeptoIsoAsync(depto_iso);

                return Ok(lasPlantasAsociadas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{ubicacion_id:Guid}/plantas")]
        public async Task<IActionResult> GetAssociatedPlantsByIdAsync(Guid ubicacion_id)
        {
            try
            {
                var lasPlantasAsociadas = await _ubicacionService
                    .GetAssociatedPlantsByIdAsync(ubicacion_id);

                return Ok(lasPlantasAsociadas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
