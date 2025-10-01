using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IProduccionRepository
    {
        public Task<IEnumerable<Produccion>> GetAllAsync();
        public Task<List<Produccion>> GetAllByPlantIdAsync(string plantaId);
        public Task<List<Produccion>> GetAllByDateIdAsync(string fechaId);
        public Task<Produccion> GetByIdAsync(string eventoId);
        public Task<Produccion> GetByDetailsAsync(Produccion unEvento);
        //public Task<bool> CreateAsync(Produccion unEvento);
        //public Task<bool> UpdateAsync(Produccion unEvento);
        //public Task<bool> RemoveAsync(Guid eventoId);
    }
}
