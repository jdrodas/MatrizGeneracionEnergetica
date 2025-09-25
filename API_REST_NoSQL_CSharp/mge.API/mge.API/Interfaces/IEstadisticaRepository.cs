using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface IEstadisticaRepository
    {
        public Task<Estadistica> GetAllAsync();
    }
}
