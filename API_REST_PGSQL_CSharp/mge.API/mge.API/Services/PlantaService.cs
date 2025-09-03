using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class PlantaService(IPlantaRepository plantaRepository)
    {
        private readonly IPlantaRepository _plantaRepository = plantaRepository;

        public async Task<List<Planta>> GetAllAsync()
        {
            return await _plantaRepository
                .GetAllAsync();
        }

        public async Task<Planta> GetByIdAsync(Guid planta_id)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(planta_id);

            if (unaPlanta.Id == Guid.Empty)
                throw new AppValidationException($"Planta no encontrada con el Id {planta_id}");

            return unaPlanta;
        }
    }
}
