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
    }
}
