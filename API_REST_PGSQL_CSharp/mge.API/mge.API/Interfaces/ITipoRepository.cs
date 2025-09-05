using mge.API.Models;

namespace mge.API.Interfaces
{
    public interface ITipoRepository
    {
        public Task<List<Tipo>> GetAllAsync();
        public Task<Tipo> GetByIdAsync(Guid tipo_id);
        public Task<Tipo> GetByDetailsAsync(Tipo unTipo);
        public Task<bool> CreateAsync(Tipo unTipo);
        public Task<bool> UpdateAsync(Tipo unTipo);
        public Task<bool> RemoveAsync(Guid tipo_id);
    }
}
