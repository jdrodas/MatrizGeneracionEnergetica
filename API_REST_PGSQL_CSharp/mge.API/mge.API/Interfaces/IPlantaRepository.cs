using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IPlantaRepository
    {
        public Task<List<Planta>> GetAllAsync();
        public Task<List<Planta>> GetAllByLocationIdAsync(Guid ubicacionId);
        public Task<List<Planta>> GetAllByTypeIdAsync(Guid tipoId);
        public Task<Planta> GetByIdAsync(Guid tipoId);
        public Task<Planta> GetByDetailsAsync(string planta_nombre, Guid ubicacionId, Guid tipoId);
        public Task<bool> CreateAsync(Planta unaPlanta);
        public Task<bool> UpdateAsync(Planta unaPlanta);
        public Task<bool> RemoveAsync(Guid plantaId);
    }
}
