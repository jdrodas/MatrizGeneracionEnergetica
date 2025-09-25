using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/ubicaciones")]
    public class UbicacionesController(UbicacionService ubicacionService) : Controller
    {
        private readonly UbicacionService _ubicacionService = ubicacionService;

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] UbicacionParametrosConsulta ubicacionParametrosConsulta)
        {
            // Si se especifica por Id de la ubicación
            if (ubicacionParametrosConsulta.Id != Guid.Empty)
            {
                try
                {
                    ubicacionParametrosConsulta.Criterio = "id";

                    var unaUbicacionDetallada = await _ubicacionService
                        .GetLocationDetailsByIdAsync(ubicacionParametrosConsulta.Id);

                    return Ok(unaUbicacionDetallada);
                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }
            }

            // Si se especifica por nombre de la ubicación (municipio, departamento)
            if (!string.IsNullOrEmpty(ubicacionParametrosConsulta.Nombre))
            {
                try
                {
                    ubicacionParametrosConsulta.Criterio = "nombre";

                    var unaUbicacion = await _ubicacionService
                        .GetByNameAsync(ubicacionParametrosConsulta.Nombre);

                    return Ok(unaUbicacion);
                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }
            }

            // Si se especifica por ISO del departamento
            if (!string.IsNullOrEmpty(ubicacionParametrosConsulta.DeptoIso))
            {
                try
                {
                    ubicacionParametrosConsulta.Criterio = "deptoIso";

                    var ubicacionesIsoDepto = await _ubicacionService
                        .GetAllByDeptoIsoAsync(ubicacionParametrosConsulta.DeptoIso);

                    return Ok(ubicacionesIsoDepto);
                }
                catch (AppValidationException error)
                {
                    return BadRequest(error.Message);
                }
            }

            // De lo contrario, se obtienen todas las ubicaciones
            var lasUbicaciones = await _ubicacionService
                    .GetAllAsync();

            return Ok(lasUbicaciones);
        }

        [HttpGet("{ubicacionId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid ubicacionId)
        {
            try
            {
                var unaUbicacionDetallada = await _ubicacionService
                    .GetLocationDetailsByIdAsync(ubicacionId);

                return Ok(unaUbicacionDetallada);
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
