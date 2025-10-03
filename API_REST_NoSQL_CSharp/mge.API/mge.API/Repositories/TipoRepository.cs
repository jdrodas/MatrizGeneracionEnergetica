using mge.API.DbContexts;
using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.Repositories
{
    public class TipoRepository(MongoDbContext unContexto) : ITipoRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Tipo>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var losTipos = await coleccionTipos
                .Find(_ => true)
                .SortBy(tipo => tipo.Nombre)
                .ToListAsync();

            return losTipos;
        }

        public async Task<Tipo> GetByIdAsync(string tipoId)
        {
            Tipo unTipo = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var resultado = await coleccionTipos
                .Find(tipo => tipo.Id == tipoId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unTipo = resultado;

            return unTipo;
        }

        public async Task<Tipo> GetByNameAsync(string tipoNombre)
        {
            Tipo unTipo = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var resultado = await coleccionTipos
                .Find(tipo => tipo.Nombre!.ToLower() == tipoNombre.ToLower())
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unTipo = resultado;

            return unTipo;
        }

        public async Task<Tipo> GetByDetailsAsync(Tipo unTipo)
        {
            Tipo tipoExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var builder = Builders<Tipo>.Filter;
            
            //TODO: Aplicar comparación invariante de mayúsculas y minúsculas para el nombre y la descripción
            var filtro = builder.And(
                builder.Eq(tipo => tipo.Nombre, unTipo.Nombre),
                builder.Eq(tipo => tipo.Descripcion, unTipo.Descripcion),
                builder.Eq(tipo => tipo.EsRenovable, unTipo.EsRenovable));

            var resultado = await coleccionTipos
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                tipoExistente = resultado;

            return tipoExistente;
        }

        public async Task<Tipo> GetByDetailsAsync(string tipoId, string tipoNombre)
        {
            Tipo tipoExistente = new();

            if (string.IsNullOrEmpty(tipoNombre) && string.IsNullOrEmpty(tipoId))
                throw new AppValidationException("Datos insuficientes para obtener el tipo");

            if (!string.IsNullOrEmpty(tipoNombre) && string.IsNullOrEmpty(tipoId))
                tipoExistente = await GetByNameAsync(tipoNombre!);

            if (!string.IsNullOrEmpty(tipoId))
                tipoExistente = await GetByIdAsync(tipoId);

            return tipoExistente;
        }

        public async Task<bool> CreateAsync(Tipo unTipo)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();
            
            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            await coleccionTipos
                .InsertOneAsync(unTipo);

            var resultado = await GetByDetailsAsync(unTipo);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Tipo unTipo)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();
            
            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var resultado = await coleccionTipos
                .ReplaceOneAsync(tipo => tipo.Id == unTipo.Id, unTipo);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }        

        public async Task<bool> RemoveAsync(string tipoId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();
            
            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var resultado = await coleccionTipos
                .DeleteOneAsync(tipo => tipo.Id == tipoId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
