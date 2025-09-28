using Asp.Versioning;
using mge.API.Exceptions;
using mge.API.Models;
using mge.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace mge.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/ubicaciones")]
    public class UbicacionesController(UbicacionService ubicacionService) : Controller
    {
        private readonly UbicacionService _ubicacionService = ubicacionService;

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var lasUbicaciones = await _ubicacionService
        //        .GetAllAsync();

        //    return Ok(lasUbicaciones);
        //}

        [HttpGet]
        public async Task<IActionResult> GetDetailsByParameterAsync([FromQuery] UbicacionParametrosConsulta ubicacionParametrosConsulta)
        {
            // Si se especifica por Id de la ubicación
            if (!string.IsNullOrEmpty(ubicacionParametrosConsulta.Id))
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

        [HttpGet("{ubicacionId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string ubicacionId)
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

        [HttpGet("{ubicacionId:length(24)}/plantas")]
        public async Task<IActionResult> GetAssociatedPlantsByIdAsync(string ubicacionId)
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
