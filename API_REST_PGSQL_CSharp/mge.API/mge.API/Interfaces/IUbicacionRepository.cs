using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<List<Ubicacion>> GetAllAsync();

        public Task<Ubicacion> GetByIdAsync(Guid ubicacion_id);
    }
}
