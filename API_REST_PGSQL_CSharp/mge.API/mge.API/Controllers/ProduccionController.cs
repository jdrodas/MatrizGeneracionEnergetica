using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
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
