using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<IEnumerable<Ubicacion>> GetAllAsync();
        public Task<IEnumerable<Ubicacion>> GetAllByDeptoIsoAsync(string deptoIso);
        public Task<Ubicacion> GetByIdAsync(Guid ubicacionId);
        public Task<Ubicacion> GetByNameAsync(string ubicacionNombre);
        public Task<Ubicacion> GetByDetailsAsync(Guid ubicacionId, string ubicacionNombre);
    }
}
