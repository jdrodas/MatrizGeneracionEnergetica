using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class PlantaService(IPlantaRepository plantaRepository, 
        ITipoRepository tipoRepository,
        IUbicacionRepository ubicacionRepository,
        IProduccionRepository produccionRepository)
    {
        private readonly IPlantaRepository _plantaRepository = plantaRepository;
        private readonly ITipoRepository _tipoRepository = tipoRepository;
        private readonly IUbicacionRepository _ubicacionRepository = ubicacionRepository;
        private readonly IProduccionRepository _produccionRepository = produccionRepository;

        public async Task<PlantaRespuesta> GetAllAsync(PlantaParametrosConsulta plantaParametrosConsulta)
        {
            var lasPlantas = await _plantaRepository
                .GetAllAsync();

            var respuestaPlantas = BuildPlantResponse(lasPlantas, plantaParametrosConsulta);

            return respuestaPlantas;
        }

        public async Task<Planta> GetByIdAsync(string plantaId)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(plantaId);

            if (string.IsNullOrEmpty(unaPlanta.Id))
                throw new AppValidationException($"Planta no encontrada con el Id {plantaId}");

            return unaPlanta;
        }

        public async Task<Planta> CreateAsync(Planta unaPlanta)
        {
            unaPlanta.Nombre = unaPlanta.Nombre!.Trim();
            unaPlanta.UbicacionNombre = unaPlanta.UbicacionNombre!.Trim();
            unaPlanta.TipoNombre = unaPlanta.TipoNombre!.Trim();

            string resultadoValidacion = EvaluatePlantDetailsAsync(unaPlanta);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos si los datos del tipo son válidos
            var tipoExistente = await _tipoRepository
                .GetByDetailsAsync(unaPlanta.TipoId!, unaPlanta.TipoNombre);

            //Si el tipo no es válido, no se puede insertar la planta
            if (string.IsNullOrEmpty(tipoExistente.Id))
                throw new AppValidationException($"Inserción fallida - Datos del tipo de fuente son inválidos");

            //Validamos si los datos de la ubicación son válidos
            var ubicacionExistente = await _ubicacionRepository
                .GetByDetailsAsync(unaPlanta.UbicacionId!, unaPlanta.UbicacionNombre);

            //Si la ubicación no es válida, no se puede insertar la planta
            if (string.IsNullOrEmpty(ubicacionExistente.Id))
                throw new AppValidationException($"Inserción fallida - Datos de la ubicación de la planta son inválidos");

            unaPlanta.UbicacionId = ubicacionExistente.Id;
            unaPlanta.TipoId = tipoExistente.Id;

            var plantaExistente = await _plantaRepository
                .GetByDetailsAsync(unaPlanta.Nombre, unaPlanta.UbicacionId, unaPlanta.TipoId);

            //Si ya existe una planta con ese nombre, de ese tipo en esa ubicación, se devuelve el objeto encontrado
            if (!string.IsNullOrEmpty(plantaExistente.Id))
                return plantaExistente;

            try
            {
                bool resultadoAccion = await _plantaRepository
                    .CreateAsync(unaPlanta);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                plantaExistente = await _plantaRepository
                    .GetByDetailsAsync(unaPlanta.Nombre, unaPlanta.UbicacionId, unaPlanta.TipoId);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return plantaExistente;
        }

        public async Task<Planta> UpdateAsync(Planta unaPlanta)
        {
            unaPlanta.Nombre = unaPlanta.Nombre!.Trim();
            unaPlanta.UbicacionNombre = unaPlanta.UbicacionNombre!.Trim();
            unaPlanta.TipoNombre = unaPlanta.TipoNombre!.Trim();

            string resultadoValidacion = EvaluatePlantDetailsAsync(unaPlanta);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos si los datos del tipo son válidos
            var tipoExistente = await _tipoRepository
                .GetByDetailsAsync(unaPlanta.TipoId!, unaPlanta.TipoNombre);

            //Si el tipo no es válido, no se puede insertar la planta
            if (string.IsNullOrEmpty(tipoExistente.Id))
                throw new AppValidationException($"Actualización fallida - Datos del tipo de fuente son inválidos");

            //Validamos si los datos de la ubicación son válidos
            var ubicacionExistente = await _ubicacionRepository
                .GetByDetailsAsync(unaPlanta.UbicacionId!, unaPlanta.UbicacionNombre!);

            //Si la ubicación no es válida, no se puede insertar la planta
            if (string.IsNullOrEmpty(ubicacionExistente.Id))
                throw new AppValidationException($"Inserción fallida - Datos de la ubicación de la planta son inválidos");

            unaPlanta.UbicacionId = ubicacionExistente.Id;
            unaPlanta.TipoId = tipoExistente.Id;

            //Validamos si hay una planta con ese Id
            var plantaExistente = await _plantaRepository
                .GetByIdAsync(unaPlanta.Id!);

            if (string.IsNullOrEmpty(plantaExistente.Id))
                throw new EmptyCollectionException($"Actualización fallida - No hay planta registrada con el Id {unaPlanta.Id}");

            //Si existe y los datos son iguales, se retorna el objeto para garantizar idempotencia
            if (plantaExistente.Equals(unaPlanta))
                return plantaExistente;

            try
            {
                bool resultadoAccion = await _plantaRepository
                    .UpdateAsync(unaPlanta);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                plantaExistente = await _plantaRepository
                    .GetByIdAsync(unaPlanta.Id!);

                resultadoAccion = await _produccionRepository
                    .UpdatePlantAsync(plantaExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("No se actualizaron eventos de producción relacionados a esta planta");

            }
            catch (DbOperationException)
            {
                throw;
            }

            return plantaExistente;
        }

        public async Task<string> RemoveAsync(string plantaId)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(plantaId);

            if (string.IsNullOrEmpty(unaPlanta.Id))
                throw new EmptyCollectionException($"Planta no encontrada con el id {plantaId}");

            //Validar si la planta tiene producción asociada
            var produccion_asociada = await _produccionRepository
                .GetAllByPlantIdAsync(plantaId);

            if (produccion_asociada.Count != 0)
                throw new AppValidationException($"la planta {unaPlanta.Nombre} no se puede eliminar porque tiene {produccion_asociada.Count} eventos de producción asociados");

            string nombrePlantaEliminada = unaPlanta.Nombre!;

            try
            {
                bool resultadoAccion = await _plantaRepository
                    .RemoveAsync(plantaId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombrePlantaEliminada;
        }

        private static string EvaluatePlantDetailsAsync(Planta unaPlanta)
        {
            if (string.IsNullOrEmpty(unaPlanta.Nombre))
                return "No se puede insertar una planta con nombre nulo";

            if (string.IsNullOrEmpty(unaPlanta.TipoNombre) && string.IsNullOrEmpty(unaPlanta.TipoId))
                return "No se puede insertar una planta sin información del tipo de fuente";

            if (string.IsNullOrEmpty(unaPlanta.UbicacionNombre) && string.IsNullOrEmpty(unaPlanta.UbicacionId))
                return "No se puede insertar una planta sin información de la ubicación";

            if (unaPlanta.Capacidad <= 0)
                return "La capacidad de la planta en MW debe ser mayor que 0";

            return string.Empty;
        }

        private static PlantaRespuesta BuildPlantResponse(IEnumerable<Planta> lasPlantas, PlantaParametrosConsulta plantaParametrosConsulta)
        {
            // Calculamos items totales y cantidad de páginas
            var totalElementos = lasPlantas.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / plantaParametrosConsulta.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (plantaParametrosConsulta.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {plantaParametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {plantaParametrosConsulta.ElementosPorPagina}");

            //Aplicamos la paginación
            lasPlantas = lasPlantas
                .Skip((plantaParametrosConsulta.Pagina - 1) * plantaParametrosConsulta.ElementosPorPagina)
                .Take(plantaParametrosConsulta.ElementosPorPagina);

            var respuestaPlantas = new PlantaRespuesta
            {
                Tipo = "Planta",
                TotalElementos = totalElementos,
                Pagina = plantaParametrosConsulta.Pagina, // page
                ElementosPorPagina = plantaParametrosConsulta.ElementosPorPagina, // pageSize
                TotalPaginas = totalPaginas,
                Criterio = plantaParametrosConsulta.Criterio,
                Data = [.. lasPlantas]
            };

            return respuestaPlantas;
        }
    }
}
