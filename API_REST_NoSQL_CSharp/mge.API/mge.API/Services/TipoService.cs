using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class TipoService(ITipoRepository tipoRepository, IPlantaRepository plantaRepository)
    {
        private readonly ITipoRepository _tipoRepository = tipoRepository;
        private readonly IPlantaRepository _plantaRepository = plantaRepository;

        public async Task<List<Tipo>> GetAllAsync()
        {
            return await _tipoRepository
                .GetAllAsync();
        }

        public async Task<Tipo> GetByIdAsync(string tipoId)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipoId);

            if (string.IsNullOrEmpty(unTipo.Id))
                throw new AppValidationException($"Tipo de fuente no encontrado con el Id {tipoId}");

            return unTipo;
        }

        public async Task<TipoDetallado> GetTypeDetailsByIdAsync(string tipoId)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipoId);

            if (string.IsNullOrEmpty(unTipo.Id))
                throw new AppValidationException($"Tipo de fuente no encontrado con el Id {tipoId}");

            TipoDetallado unTipoDetallado = new()
            {
                Id = unTipo.Id,
                Nombre = unTipo.Nombre,
                Descripcion = unTipo.Descripcion,
                EsRenovable = unTipo.EsRenovable,
                Plantas = await _plantaRepository.GetAllByTypeIdAsync(tipoId)
            };

            unTipoDetallado.TotalPlantas = unTipoDetallado.Plantas.Count;

            return unTipoDetallado;
        }

        public async Task<List<Planta>> GetAssociatedPlantsAsync(string tipoId)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipoId);

            if (string.IsNullOrEmpty(unTipo.Id))
                throw new AppValidationException($"Tipo de fuente no encontrado con el Id {tipoId}");

            var plantasAsociadas = await _plantaRepository
                .GetAllByTypeIdAsync(tipoId);

            if (plantasAsociadas.Count == 0)
                throw new EmptyCollectionException($"Tipo {unTipo.Nombre} no tiene plantas asociadas");

            return plantasAsociadas;
        }

        public async Task<Tipo> CreateAsync(Tipo unTipo)
        {
            unTipo.Nombre = unTipo.Nombre!.Trim();
            unTipo.Descripcion = unTipo.Descripcion!.Trim();

            string resultadoValidacion = EvaluateTypeDetailsAsync(unTipo);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos primero si existe con ese nombre, descripcion y si es renovable
            var tipoExistente = await _tipoRepository
                .GetByDetailsAsync(unTipo);

            //Si existe y los datos son iguales, se retorna el objeto para garantizar idempotencia
            if (tipoExistente.Nombre!.ToLower() == unTipo.Nombre!.ToLower() &&
                tipoExistente.Descripcion!.ToLower() == unTipo.Descripcion!.ToLower() &&
                tipoExistente.EsRenovable == unTipo.EsRenovable)
                return tipoExistente;

            try
            {
                bool resultadoAccion = await _tipoRepository
                    .CreateAsync(unTipo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                tipoExistente = await _tipoRepository
                .GetByDetailsAsync(unTipo);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return tipoExistente;
        }

        public async Task<Tipo> UpdateAsync(Tipo unTipo)
        {
            unTipo.Nombre = unTipo.Nombre!.Trim();
            unTipo.Descripcion = unTipo.Descripcion!.Trim();

            string resultadoValidacion = EvaluateTypeDetailsAsync(unTipo);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos primero si existe un tipo con ese Id
            var tipoExistente = await _tipoRepository
                .GetByIdAsync(unTipo.Id!);

            if (string.IsNullOrEmpty(tipoExistente.Id))
                throw new AppValidationException($"No existe un tipo con el Guid {unTipo.Id} que se pueda actualizar");

            //Si existe y los datos son iguales, se retorna el objeto para garantizar idempotencia
            if (tipoExistente.Equals(unTipo))
                return tipoExistente;

            try
            {
                bool resultadoAccion = await _tipoRepository
                    .UpdateAsync(unTipo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                tipoExistente = await _tipoRepository
                    .GetByIdAsync(unTipo.Id!);

                resultadoAccion = await _plantaRepository
                    .UpdateSourceTypeAsync(tipoExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("No se actualizaron plantas relacionadas");

            }
            catch (DbOperationException)
            {
                throw;
            }

            return tipoExistente;
        }

        public async Task<string> RemoveAsync(string tipoId)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipoId);

            if (string.IsNullOrEmpty(unTipo.Id))
                throw new AppValidationException($"Tipo no encontrado con el id {tipoId}");

            //Validar si el tipo de fuente tiene plantas asociadas
            var plantasAsociadas = await _plantaRepository
                .GetAllByTypeIdAsync(tipoId);

            if (plantasAsociadas.Count != 0)
                throw new AppValidationException($"El tipo {unTipo.Nombre} no se puede eliminar porque tiene {plantasAsociadas.Count} plantas asociadas");

            string nombreTipoEliminado = unTipo.Nombre!;

            try
            {
                bool resultadoAccion = await _tipoRepository
                    .RemoveAsync(tipoId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreTipoEliminado;
        }

        private static string EvaluateTypeDetailsAsync(Tipo unTipo)
        {
            if (unTipo.Nombre!.Length == 0)
                return "No se puede insertar un tipo con nombre nulo";

            if (unTipo.Descripcion!.Length == 0)
                return "No se puede insertar un tipo con la descripción nula.";

            return string.Empty;
        }
    }
}
