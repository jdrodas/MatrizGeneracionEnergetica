using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using mge.API.Repositories;

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

        public async Task<Tipo> GetByIdAsync(Guid tipo_id)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipo_id);

            if (unTipo.Id == Guid.Empty)
                throw new AppValidationException($"Tipo de fuente no encontrado con el Id {tipo_id}");

            return unTipo;
        }

        public async Task<List<Planta>> GetAssociatedPlantsAsync(Guid tipo_id)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipo_id);

            if (unTipo.Id == Guid.Empty)
                throw new AppValidationException($"Tipo de fuente no encontrado con el Id {tipo_id}");

            var plantasAsociadas = await _plantaRepository
                .GetAllByTypeIdAsync(tipo_id);

            if (plantasAsociadas.Count == 0)
                throw new AppValidationException($"Tipo {unTipo.Nombre} no tiene plantas asociadas");

            return plantasAsociadas;
        }

        public async Task<Tipo> CreateAsync(Tipo unTipo)
        {
            unTipo.Nombre = unTipo.Nombre!.Trim();
            unTipo.Descripcion = unTipo.Descripcion!.Trim();
            
            string resultadoValidacion = EvaluateTypeDetailsAsync(unTipo);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos primero si existe con ese nombre y descripcion
            var tipoExistente = await _tipoRepository
                .GetByDetailsAsync(unTipo);

            //Si existe y los datos son iguales, se retorna el objeto para garantizar idempotencia
            if (tipoExistente.Nombre == unTipo.Nombre! &&
                tipoExistente.Descripcion == unTipo.Descripcion &&
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

            //Validamos primero si existe con ese Id
            var tipoExistente = await _tipoRepository
                .GetByIdAsync(unTipo.Id);

            if (tipoExistente.Id == Guid.Empty)
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
                    .GetByIdAsync(unTipo.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return tipoExistente;
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
