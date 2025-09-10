using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IProduccionRepository
    {
        public Task<List<Produccion>> GetAllAsync();
        public Task<List<Produccion>> GetAllByPlantIdAsync(Guid plantaId);
        public Task<List<Produccion>> GetAllByDateIdAsync(string fechaId);
        public Task<Produccion> GetByIdAsync(Guid eventoId);
    }
}
