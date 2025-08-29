using mge.API.Exceptions;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers
{
    [ApiController]
    [Route("api/v1/tipos")]
    public class TiposController(TipoService tipoService) : Controller
    {
        private readonly TipoService _tipoService = tipoService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losTipos = await _tipoService
                .GetAllAsync();

            return Ok(losTipos);
        }

        [HttpGet("{tipo_id:Guid}")]
        public async Task<IActionResult> GetByGuidAsync(Guid tipo_id)
        {
            try
            {
                var unTipo = await _tipoService
                    .GetByIdAsync(tipo_id);

                return Ok(unTipo);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
