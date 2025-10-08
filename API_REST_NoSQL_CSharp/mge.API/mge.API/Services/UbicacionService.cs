using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class UbicacionService(IUbicacionRepository ubicacionRepository, IPlantaRepository plantaRepository)
    {
        private readonly IUbicacionRepository _ubicacionRepository = ubicacionRepository;
        private readonly IPlantaRepository _plantaRepository = plantaRepository;

        public async Task<UbicacionRespuesta> GetAllAsync(UbicacionParametrosConsulta ubicacionParametrosConsulta)
        {
            var lasUbicaciones = await _ubicacionRepository
                .GetAllAsync();

            var respuestaUbicaciones = BuildLocationResponse(lasUbicaciones, ubicacionParametrosConsulta);

            return respuestaUbicaciones;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            return await _ubicacionRepository
                .GetAllAsync();
        }

        public async Task<UbicacionRespuesta> GetAllByDeptoIsoAsync(string deptoIso, UbicacionParametrosConsulta ubicacionParametrosConsulta)
        {
            var lasUbicaciones = await _ubicacionRepository
                .GetAllByDeptoIsoAsync(deptoIso);

            var respuestaUbicaciones = BuildLocationResponse(lasUbicaciones, ubicacionParametrosConsulta);

            return respuestaUbicaciones;

        }

        public async Task<Ubicacion> GetByIdAsync(string ubicacionId)
        {
            try
            {
                Ubicacion unaUbicacion = await _ubicacionRepository
                    .GetByIdAsync(ubicacionId);

                if (string.IsNullOrEmpty(unaUbicacion.Id))
                    throw new AppValidationException($"Ubicación no encontrada con el Id {ubicacionId}");

                return unaUbicacion;
            }
            catch (FormatException error)
            {
                throw new AppValidationException($"La cadena de caracteres no representa un Id válido. {error.Message}");
            }
        }

        public async Task<UbicacionDetallada> GetLocationDetailsByIdAsync(string ubicacionId)
        {
            try 
            {
                Ubicacion unaUbicacion = await _ubicacionRepository
                    .GetByIdAsync(ubicacionId);

                if (string.IsNullOrEmpty(unaUbicacion.Id))
                    throw new EmptyCollectionException($"Ubicación no encontrada con el Id {ubicacionId}");

                UbicacionDetallada unaUbicacionDetallada = new()
                {
                    Id = unaUbicacion.Id,
                    CodigoDepartamento = unaUbicacion.CodigoDepartamento,
                    IsoDepartamento = unaUbicacion.IsoDepartamento,
                    NombreDepartamento = unaUbicacion.NombreDepartamento,
                    CodigoMunicipio = unaUbicacion.CodigoMunicipio,
                    NombreMunicipio = unaUbicacion.NombreMunicipio,
                    Plantas = await _plantaRepository.GetAllByLocationIdAsync(ubicacionId)
                };

                unaUbicacionDetallada.TotalPlantas = unaUbicacionDetallada.Plantas.Count;

                return unaUbicacionDetallada;
            }
            catch (FormatException error)
            {
                throw new AppValidationException($"La cadena de caracteres no representa un Id válido. {error.Message}");
            }
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacionNombre)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByNameAsync(ubicacionNombre);

            if (string.IsNullOrEmpty(unaUbicacion.Id))
                throw new EmptyCollectionException($"Ubicación no encontrada con el nombre {ubicacionNombre}");

            return unaUbicacion;
        }

        public async Task<List<Planta>> GetAssociatedPlantsByIdAsync(string ubicacionId)
        {
            try
            {
                Ubicacion unaUbicacion = await _ubicacionRepository
                    .GetByIdAsync(ubicacionId);

                if (string.IsNullOrEmpty(unaUbicacion.Id))
                    throw new EmptyCollectionException($"Ubicación no encontrada con el Id {ubicacionId}");

                var plantasAsociadas = await _plantaRepository
                    .GetAllByLocationIdAsync(ubicacionId);

                if (plantasAsociadas.Count == 0)
                    throw new EmptyCollectionException($"Ubicación {unaUbicacion.NombreMunicipio}, {unaUbicacion.NombreDepartamento} no tiene plantas asociadas");

                return plantasAsociadas;
            }
            catch (FormatException error)
            {
                throw new AppValidationException($"La cadena de caracteres no representa un Id válido. {error.Message}");
            }
        }

        private static UbicacionRespuesta BuildLocationResponse(IEnumerable<Ubicacion> lasUbicaciones, UbicacionParametrosConsulta ubicacionParametrosConsulta)
        {
            // Calculamos items totales y cantidad de páginas
            var totalElementos = lasUbicaciones.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / ubicacionParametrosConsulta.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (ubicacionParametrosConsulta.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {ubicacionParametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {ubicacionParametrosConsulta.ElementosPorPagina}");

            //Aplicamos la paginación
            lasUbicaciones = lasUbicaciones
                .Skip((ubicacionParametrosConsulta.Pagina - 1) * ubicacionParametrosConsulta.ElementosPorPagina)
                .Take(ubicacionParametrosConsulta.ElementosPorPagina);

            var respuestaUbicaciones = new UbicacionRespuesta
            {
                Tipo = "Ubicación",
                TotalElementos = totalElementos,
                Pagina = ubicacionParametrosConsulta.Pagina, // page
                ElementosPorPagina = ubicacionParametrosConsulta.ElementosPorPagina, // pageSize
                TotalPaginas = totalPaginas,
                Criterio = ubicacionParametrosConsulta.Criterio,
                Data = [.. lasUbicaciones]
            };

            return respuestaUbicaciones;
        }
    }
}
