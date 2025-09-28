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

        public async Task<List<Ubicacion>> GetAllByDeptoIsoAsync(string deptoIso)
        {
            return await _ubicacionRepository
                .GetAllByDeptoIsoAsync(deptoIso);
        }

        public async Task<Ubicacion> GetByIdAsync(string ubicacionId)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacionId);

            if (string.IsNullOrEmpty(unaUbicacion.Id))
                throw new AppValidationException($"Ubicación no encontrada con el Id {ubicacionId}");

            return unaUbicacion;
        }

        public async Task<UbicacionDetallada> GetLocationDetailsByIdAsync(string ubicacionId)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacionId);

            if (string.IsNullOrEmpty(unaUbicacion.Id))
                throw new AppValidationException($"Ubicación no encontrada con el Id {ubicacionId}");

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

        public async Task<Ubicacion> GetByNameAsync(string ubicacionNombre)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByNameAsync(ubicacionNombre);

            if (string.IsNullOrEmpty(unaUbicacion.Id))
                throw new AppValidationException($"Ubicación no encontrada con el nombre {ubicacionNombre}");

            return unaUbicacion;
        }

        public async Task<List<Planta>> GetAssociatedPlantsByIdAsync(string ubicacionId)
        {
            Ubicacion unaUbicacion = await _ubicacionRepository
                .GetByIdAsync(ubicacionId);

            if (string.IsNullOrEmpty(unaUbicacion.Id))
                throw new AppValidationException($"Ubicación no encontrada con el Id {ubicacionId}");

            var plantasAsociadas = await _plantaRepository
                .GetAllByLocationIdAsync(ubicacionId);

            if (plantasAsociadas.Count == 0)
                throw new AppValidationException($"Ubicación {unaUbicacion.NombreMunicipio}, {unaUbicacion.NombreDepartamento} no tiene plantas asociadas");

            return plantasAsociadas;
        }
    }
}
