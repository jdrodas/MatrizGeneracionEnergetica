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
            Tipo unTIpo = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionTipos = conexion
                .GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);

            var resultado = await coleccionTipos
                .Find(tipo => tipo.Id == tipoId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unTIpo = resultado;

            return unTIpo;
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
            var filtro = builder.And(
                builder.Eq(tipo => tipo.Nombre!.ToLower(), unTipo.Nombre!.ToLower()),
                builder.Eq(tipo => tipo.Descripcion!.ToLower(), unTipo.Descripcion!.ToLower()),
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

        //public async Task<bool> CreateAsync(Tipo unTipo)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_inserta_tipo";
        //        var parametros = new
        //        {
        //            p_nombre = unTipo.Nombre,
        //            p_descripcion = unTipo.Descripcion,
        //            p_esrenovable = unTipo.EsRenovable
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //public async Task<bool> UpdateAsync(Tipo unTipo)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_actualiza_tipo";
        //        var parametros = new
        //        {
        //            p_id = unTipo.Id,
        //            p_nombre = unTipo.Nombre,
        //            p_descripcion = unTipo.Descripcion,
        //            p_esrenovable = unTipo.EsRenovable
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //public async Task<bool> RemoveAsync(Guid tipoId)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_elimina_tipo";
        //        var parametros = new
        //        {
        //            p_id = tipoId
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}
    }
}
