using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using mge.API.Repositories;

namespace mge.API.Services
{
    public class ProducccionService(IProduccionRepository produccionRepository)
    {
        private readonly IProduccionRepository _produccionRepository = produccionRepository;

        public async Task<List<Produccion>> GetAllAsync()
        {
            return await _produccionRepository
                .GetAllAsync();
        }

        public async Task<Produccion> GetByIdAsync(Guid evento_id)
        {
            Produccion unEvento = await _produccionRepository
                .GetByIdAsync(evento_id);

            if (unEvento.Id == Guid.Empty)
                throw new AppValidationException($"Evento de producción no encontrado con el Id {evento_id}");

            return unEvento;
        }
    }
}
