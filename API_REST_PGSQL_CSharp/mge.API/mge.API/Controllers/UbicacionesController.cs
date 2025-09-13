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

        [HttpGet("{ubicacionId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid ubicacionId)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                    .GetByIdAsync(ubicacionId);

                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }       

        [HttpGet("{ubicacionId:Guid}/plantas")]
        public async Task<IActionResult> GetAssociatedPlantsByIdAsync(Guid ubicacionId)
        {
            try
            {
                var lasPlantasAsociadas = await _ubicacionService
                    .GetAssociatedPlantsByIdAsync(ubicacionId);

                return Ok(lasPlantasAsociadas);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }        
    }
}
