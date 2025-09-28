using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<List<Ubicacion>> GetAllAsync();
        public Task<List<Ubicacion>> GetAllByDeptoIsoAsync(string deptoIso);
        public Task<Ubicacion> GetByIdAsync(string ubicacionId);
        public Task<Ubicacion> GetByNameAsync(string ubicacionNombre);
        public Task<Ubicacion> GetByDetailsAsync(string ubicacionId, string ubicacionNombre);
    }
}
