using Dapper;
using mge.API.DbContexts;
using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using Npgsql;
using System.Data;

namespace mge.API.Repositories
{
    public class TipoRepository(PgsqlDbContext unContexto) : ITipoRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Tipo>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos ORDER BY nombre";

            var resultadoTipos = await conexion
                .QueryAsync<Tipo>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoTipos];
        }

        public async Task<Tipo> GetByIdAsync(Guid tipoId)
        {
            Tipo unTipo = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipoId", tipoId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos " +
                "WHERE id = @tipoId";

            var resultado = await conexion
                .QueryAsync<Tipo>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unTipo = resultado.First();

            return unTipo;
        }

        public async Task<Tipo> GetByNameAsync(string tipoNombre)
        {
            Tipo unTipo = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipoNombre", tipoNombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos " +
                "WHERE nombre = @tipoNombre";

            var resultado = await conexion
                .QueryAsync<Tipo>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unTipo = resultado.First();

            return unTipo;
        }

        public async Task<Tipo> GetByDetailsAsync(Tipo unTipo)
        {
            Tipo tipoExistente = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipoNombre", unTipo.Nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@tipoDescripcion", unTipo.Descripcion,
                        DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@tipoEsRenovable", unTipo.EsRenovable,
                        DbType.Boolean, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos " +
                "WHERE id = @tipoId " +
                "AND LOWER(nombre) = LOWER(@tipoNombre) " +
                "AND LOWER(descripcion) = LOWER(@tipoDescripcion) " +
                "AND esrenovable = @tipoEsRenovable";

            var resultado = await conexion
                .QueryAsync<Tipo>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                tipoExistente = resultado.First();

            return tipoExistente;
        }

        public async Task<Tipo> GetByDetailsAsync(Guid tipoId, string tipoNombre)
        {
            Tipo tipoExistente = new();

            if (string.IsNullOrEmpty(tipoNombre) && tipoId == Guid.Empty)
                throw new AppValidationException("Datos insuficientes para obtener el tipo");

            if (!string.IsNullOrEmpty(tipoNombre) && tipoId == Guid.Empty)
                tipoExistente = await GetByNameAsync(tipoNombre!);

            if (tipoId != Guid.Empty)
                tipoExistente = await GetByIdAsync(tipoId);

            return tipoExistente;
        }

        public async Task<bool> CreateAsync(Tipo unTipo)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_tipo";
                var parametros = new
                {
                    p_nombre        = unTipo.Nombre,
                    p_descripcion   = unTipo.Descripcion,
                    p_esrenovable   = unTipo.EsRenovable
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Tipo unTipo)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_tipo";
                var parametros = new
                {
                    p_id            = unTipo.Id,
                    p_nombre        = unTipo.Nombre,
                    p_descripcion   = unTipo.Descripcion,
                    p_esrenovable   = unTipo.EsRenovable
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(Guid tipoId)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_tipo";
                var parametros = new
                {
                    p_id = tipoId
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}
