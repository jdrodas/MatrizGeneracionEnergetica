using mge.API.Models;
using System.Globalization;

namespace mge.API.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<List<Ubicacion>> GetAllAsync();

        public Task<Ubicacion> GetByIdAsync(Guid ubicacion_id);

        public Task<List<Ubicacion>> GetAllByDeptoIsoAsync(string depto_iso);
    }
}
