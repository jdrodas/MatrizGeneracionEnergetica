using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers.V1
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

        [HttpGet("{tipoId:length(24)}")]
        public async Task<IActionResult> GetTypeDetailsByIdAsync(string tipoId)
        {
            try
            {
                var unTipoDetallado = await _tipoService
                    .GetTypeDetailsByIdAsync(tipoId);

                return Ok(unTipoDetallado);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{tipoId:length(24)}/plantas")]
        public async Task<IActionResult> GetAssociatedPlantsAsync(string tipoId)
        {
            try
            {
                var lasPlantasAsociadas = await _tipoService
                    .GetAssociatedPlantsAsync(tipoId);

                return Ok(lasPlantasAsociadas);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAsync(Tipo unTipo)
        //{
        //    try
        //    {
        //        var tipoCreado = await _tipoService
        //            .CreateAsync(unTipo);

        //        return Ok(tipoCreado);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateAsync(Tipo unTipo)
        //{
        //    try
        //    {
        //        var tipoActualizado = await _tipoService
        //            .UpdateAsync(unTipo);

        //        return Ok(tipoActualizado);
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}

        //[HttpDelete("{tipoId:Guid}")]
        //public async Task<IActionResult> RemoveAsync(Guid tipoId)
        //{
        //    try
        //    {
        //        var nombreTipoBorrado = await _tipoService
        //            .RemoveAsync(tipoId);

        //        return Ok($"El tipo {nombreTipoBorrado} fue eliminado correctamente!");
        //    }
        //    catch (AppValidationException error)
        //    {
        //        return BadRequest($"Error de validación: {error.Message}");
        //    }
        //    catch (DbOperationException error)
        //    {
        //        return BadRequest($"Error de operacion en DB: {error.Message}");
        //    }
        //}
    }
}
