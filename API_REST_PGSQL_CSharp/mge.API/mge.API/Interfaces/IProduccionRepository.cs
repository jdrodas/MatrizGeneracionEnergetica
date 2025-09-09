using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IProduccionRepository
    {
        public Task<List<Produccion>> GetAllAsync();
        public Task<List<Produccion>> GetAllByPlantIdAsync(Guid planta_id);
        public Task<List<Produccion>> GetAllByDateIdAsync(string fecha_id);
        public Task<Produccion> GetByIdAsync(Guid evento_id);
    }
}
