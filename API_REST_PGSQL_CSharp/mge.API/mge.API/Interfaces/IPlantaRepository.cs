using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IPlantaRepository
    {
        public Task<List<Planta>> GetAllAsync();
        public Task<Planta> GetByIdAsync(Guid tipo_id);
    }
}
