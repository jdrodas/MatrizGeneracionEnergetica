using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Services
{
    public class TipoService(ITipoRepository tipoRepository)
    {
        private readonly ITipoRepository _tipoRepository = tipoRepository;

        public async Task<List<Tipo>> GetAllAsync()
        {
            return await _tipoRepository
                .GetAllAsync();
        }

        public async Task<Tipo> GetByIdAsync(Guid tipo_id)
        {
            Tipo unTipo = await _tipoRepository
                .GetByIdAsync(tipo_id);

            if (unTipo.Id == Guid.Empty)
                throw new AppValidationException($"Tipo de fuente no encontrado con el Id {tipo_id}");

            return unTipo;
        }
    }
}
