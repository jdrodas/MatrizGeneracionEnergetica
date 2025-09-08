using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IProduccionRepository
    {
        public Task<List<Produccion>> GetAllByPlantIdAsync(Guid planta_id);
    }
}
