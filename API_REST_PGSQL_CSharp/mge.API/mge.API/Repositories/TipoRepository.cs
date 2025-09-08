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

        public async Task<Tipo> GetByIdAsync(Guid tipo_id)
        {
            Tipo unTipo = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipo_id", tipo_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos " +
                "WHERE id = @tipo_id";

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
            parametrosSentencia.Add("@tipo_nombre", unTipo.Nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@tipo_descripcion", unTipo.Descripcion,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@tipo_esrenovable", unTipo.EsRenovable,
                                    DbType.Boolean, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos " +
                "WHERE LOWER(nombre) = LOWER(@tipo_nombre) " +
                "AND LOWER(descripcion) = LOWER(@tipo_descripcion) " +
                "AND esrenovable = @tipo_esrenovable";

            var resultado = await conexion
                .QueryAsync<Tipo>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                tipoExistente = resultado.First();

            return tipoExistente;
        }
        public async Task<Tipo> GetByDetailsAsync(Guid tipo_id, string tipo_nombre)
        {
            Tipo tipoExistente = new();
            var conexion = contextoDB.CreateConnection();

            if (string.IsNullOrEmpty(tipo_nombre) && tipo_id == Guid.Empty)
                throw new AppValidationException("Datos insuficientes para obtener el tipo de fuente");

            DynamicParameters parametrosSentencia = new();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, descripcion, esrenovable " +
                "FROM core.tipos ";

            if (!string.IsNullOrEmpty(tipo_nombre) && tipo_id == Guid.Empty)
            {
                parametrosSentencia.Add("@tipo_nombre", tipo_nombre,
                                        DbType.String, ParameterDirection.Input);

                sentenciaSQL += "WHERE LOWER(nombre) = LOWER(@tipo_nombre) ";
            }

            if (tipo_id != Guid.Empty)
            {
                parametrosSentencia.Add("@tipo_id", tipo_id,
                                        DbType.Guid, ParameterDirection.Input);

                sentenciaSQL += "WHERE id = @tipo_id";
            }

            var resultado = await conexion
                .QueryAsync<Tipo>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                tipoExistente = resultado.First();

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
                    p_nombre = unTipo.Nombre,
                    p_descripcion = unTipo.Descripcion,
                    p_esrenovable = unTipo.EsRenovable
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
                    p_id = unTipo.Id,
                    p_nombre = unTipo.Nombre,
                    p_descripcion = unTipo.Descripcion,
                    p_esrenovable = unTipo.EsRenovable
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

        public async Task<bool> RemoveAsync(Guid tipo_id)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_tipo";
                var parametros = new
                {
                    p_id = tipo_id
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
