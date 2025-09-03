using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class UbicacionService(IUbicacionRepository ubicacionRepository, IPlantaRepository plantaRepository)
    {
        private readonly IUbicacionRepository _ubicacionRepository = ubicacionRepository;
        private readonly IPlantaRepository _plantaRepository = plantaRepository;

        public async Task<List<Ubicacion>> GetAllAsync()
        {
            return await _ubicacionRepository
                .GetAllAsync();
        }

        public async Task<Ubicacion> GetByIdAsync(Guid ubicacion_id)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacion_id);

            if (unaUbicacion.Id == Guid.Empty)
                throw new AppValidationException($"Ubicación no encontrada con el Id {ubicacion_id}");

            return unaUbicacion;
        }

        public async Task<List<Planta>> GetAssociatedPlantsAsync(Guid ubicacion_id)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacion_id);

            if (unaUbicacion.Id == Guid.Empty)
                throw new AppValidationException($"Ubicación no encontrada con el Id {ubicacion_id}");

            var plantasAsociadas = await _plantaRepository
                .GetAllByLocationIdAsync(ubicacion_id);

            if (plantasAsociadas.Count == 0)
                throw new AppValidationException($"Ubicación {unaUbicacion.NombreMunicipio}, {unaUbicacion.NombreDepartamento} no tiene plantas asociadas");

            return plantasAsociadas;
        }

    }
}
