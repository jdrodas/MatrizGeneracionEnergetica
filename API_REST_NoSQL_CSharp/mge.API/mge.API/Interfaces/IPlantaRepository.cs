using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IPlantaRepository
    {
        public Task<IEnumerable<Planta>> GetAllAsync();
        public Task<List<Planta>> GetAllByLocationIdAsync(string ubicacionId);
        public Task<List<Planta>> GetAllByTypeIdAsync(string tipoId);
        public Task<Planta> GetByIdAsync(string tipoId);
        public Task<Planta> GetByDetailsAsync(string plantaNombre, string plantaId);
        public Task<Planta> GetByDetailsAsync(string plantaNombre, string ubicacionId, string tipoId);
        public Task<bool> CreateAsync(Planta unaPlanta);
        public Task<bool> UpdateAsync(Planta unaPlanta);
        public Task<bool> RemoveAsync(string plantaId);
    }
}
