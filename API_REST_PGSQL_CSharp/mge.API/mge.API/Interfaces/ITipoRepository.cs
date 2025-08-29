using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface ITipoRepository
    {
        public Task<List<Tipo>> GetAllAsync();
        public Task<Tipo> GetByIdAsync(Guid tipo_id);
    }
}
