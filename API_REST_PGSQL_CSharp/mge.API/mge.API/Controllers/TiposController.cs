using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/tipos")]
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
        public async Task<IActionResult> GetByIdAsync(Guid tipo_id)
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

        [HttpGet("{tipo_id:Guid}/plantas")]
        public async Task<IActionResult> GetAssociatedPlantsAsync(Guid tipo_id)
        {
            try
            {
                var lasPlantasAsociadas = await _tipoService
                    .GetAssociatedPlantsAsync(tipo_id);

                return Ok(lasPlantasAsociadas);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Tipo unTipo)
        {
            try
            {
                var tipoCreado = await _tipoService
                    .CreateAsync(unTipo);

                return Ok(tipoCreado);
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
