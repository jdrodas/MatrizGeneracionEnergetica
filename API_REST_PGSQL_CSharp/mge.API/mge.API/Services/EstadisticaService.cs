using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class EstadisticaService(IEstadisticaRepository estadisticaRepository)
    {
        private readonly IEstadisticaRepository _estadisticaRepository = estadisticaRepository;

        public async Task<Estadistica> GetAllAsync()
        {
            return await _estadisticaRepository
                .GetAllAsync();
        }

    }
}
