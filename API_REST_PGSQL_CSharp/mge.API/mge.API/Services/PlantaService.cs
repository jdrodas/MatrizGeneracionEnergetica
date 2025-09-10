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

        public async Task<List<Planta>> GetAllAsync()
        {
            return await _plantaRepository
                .GetAllAsync();
        }

        public async Task<Planta> GetByIdAsync(Guid plantaId)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(plantaId);

            if (unaPlanta.Id == Guid.Empty)
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
                .GetByDetailsAsync(unaPlanta.TipoId, unaPlanta.TipoNombre);

            //Si el tipo no es válido, no se puede insertar la planta
            if (tipoExistente.Id == Guid.Empty)
                throw new AppValidationException($"Inserción fallida - Datos del tipo de fuente son inválidos");

            //Validamos si los datos de la ubicación son válidos
            var ubicacionExistente = await _ubicacionRepository
                .GetByDetailsAsync(unaPlanta.UbicacionId, unaPlanta.UbicacionNombre);

            //Si la ubicación no es válida, no se puede insertar la planta
            if (ubicacionExistente.Id == Guid.Empty)
                throw new AppValidationException($"Inserción fallida - Datos de la ubicación de la planta son inválidos");

            unaPlanta.UbicacionId = ubicacionExistente.Id;
            unaPlanta.TipoId = tipoExistente.Id;

            var plantaExistente = await _plantaRepository
                .GetByDetailsAsync(unaPlanta.Nombre, unaPlanta.UbicacionId, unaPlanta.TipoId);

            //Si ya existe una planta con ese nombre, de ese tipo en esa ubicación, se devuelve el objeto encontrado
            if (plantaExistente.Id != Guid.Empty)
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
                .GetByDetailsAsync(unaPlanta.TipoId, unaPlanta.TipoNombre);

            //Si el tipo no es válido, no se puede insertar la planta
            if (tipoExistente.Id == Guid.Empty)
                throw new AppValidationException($"Actualización fallida - Datos del tipo de fuente son inválidos");

            //Validamos si los datos de la ubicación son válidos
            var ubicacionExistente = await _ubicacionRepository
                .GetByDetailsAsync(unaPlanta.UbicacionId, unaPlanta.UbicacionNombre);

            //Si la ubicación no es válida, no se puede insertar la planta
            if (ubicacionExistente.Id == Guid.Empty)
                throw new AppValidationException($"Inserción fallida - Datos de la ubicación de la planta son inválidos");

            unaPlanta.UbicacionId = ubicacionExistente.Id;
            unaPlanta.TipoId = tipoExistente.Id;

            //Validamos si hay una planta con ese Id
            var plantaExistente = await _plantaRepository
                .GetByIdAsync(unaPlanta.Id);

            if (plantaExistente.Id == Guid.Empty)
                throw new AppValidationException($"Actualización fallida - No hay planta registrada con el Id {unaPlanta.Id}");

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
                    .GetByIdAsync(unaPlanta.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return plantaExistente;
        }

        public async Task<string> RemoveAsync(Guid plantaId)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(plantaId);

            if (unaPlanta.Id == Guid.Empty)
                throw new AppValidationException($"Planta no encontrada con el id {plantaId}");

            //Validar si la planta tiene producción asociada
            var produccion_asociada = await _produccionRepository
                .GetAllByPlantIdAsync(plantaId);

            if (produccion_asociada.Count == 0)
                throw new AppValidationException($"la planta {unaPlanta.Nombre} no se puede eliminar porque tiene producción asociada");

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
            if (unaPlanta.Nombre!.Length == 0)
                return "No se puede insertar una planta con nombre nulo";

            if (unaPlanta.TipoNombre!.Length == 0 && unaPlanta.TipoId == Guid.Empty)
                return "No se puede insertar una planta sin información del tipo de fuente";

            if (unaPlanta.UbicacionNombre!.Length == 0 && unaPlanta.UbicacionId == Guid.Empty)
                return "No se puede insertar una planta sin información de la ubicación";

            if (unaPlanta.Capacidad <= 0)
                return "La capacidad de la planta en MW debe ser mayor que 0";

            return string.Empty;
        }
    }
}
