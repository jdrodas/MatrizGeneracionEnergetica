using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IPlantaRepository
    {
        public Task<List<Planta>> GetAllAsync();
        public Task<List<Planta>> GetAllByLocationIdAsync(Guid ubicacion_id);
        public Task<Planta> GetByIdAsync(Guid tipo_id);
    }
}
