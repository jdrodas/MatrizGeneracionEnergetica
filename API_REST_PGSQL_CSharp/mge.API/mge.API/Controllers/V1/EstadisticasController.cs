using Asp.Versioning;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/estadisticas")]
    public class EstadisticasController(EstadisticaService estadisticaService) : Controller
    {
        private readonly EstadisticaService _estadisticaService = estadisticaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasEstadisticas = await _estadisticaService
                .GetAllAsync();

            return Ok(lasEstadisticas);
        }
    }
}
