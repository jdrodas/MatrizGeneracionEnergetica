using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<List<Ubicacion>> GetAllAsync();
        public Task<Ubicacion> GetByIdAsync(Guid ubicacion_id);
        public Task<Ubicacion> GetByNameAsync(string ubicacion_nombre);
        public Task<Ubicacion> GetByDetailsAsync(Guid ubicacion_id, string ubicacion_nombre);
    }
}
