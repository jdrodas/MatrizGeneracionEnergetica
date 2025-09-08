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

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_nombre)
        {
            if (string.IsNullOrEmpty(ubicacion_nombre))
                throw new AppValidationException("El parametro ubicacion_nombre no puede ser nulo o vacío.");

            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByNameAsync(ubicacion_nombre);

            if (unaUbicacion.Id == Guid.Empty)
                throw new AppValidationException($"Ubicación no encontrada con el nombre {ubicacion_nombre}");


            return unaUbicacion;
        }

        public async Task<List<Planta>> GetAssociatedPlantsByDeptoIsoAsync(string depto_iso)
        {
            if (string.IsNullOrEmpty(depto_iso))
                throw new AppValidationException("El parametro depto_iso no puede ser nulo o vacío.");

            var plantasAsociadas = await _plantaRepository
                .GetAllByDeptoIsoAsync(depto_iso);

            if (plantasAsociadas.Count == 0)
                throw new AppValidationException($"En el departamento {depto_iso} no hay plantas asociadas");

            return plantasAsociadas;
        }

        public async Task<List<Planta>> GetAssociatedPlantsByIdAsync(Guid ubicacion_id)
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
