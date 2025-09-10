using Dapper;
using mge.API.DbContexts;
using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using Npgsql;
using System.Data;

namespace mge.API.Repositories
{
    public class PlantaRepository(PgsqlDbContext unContexto) : IPlantaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Planta>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "ORDER BY planta_nombre";

            var resultadoPlantas = await conexion
                .QueryAsync<Planta>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoPlantas];
        }

        public async Task<List<Planta>> GetAllByLocationIdAsync(Guid ubicacionId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacionId", ubicacionId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE ubicacion_id = @ubicacionId " +
                "ORDER BY planta_nombre";

            var resultadoPlantas = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPlantas];
        }

        public async Task<List<Planta>> GetAllByTypeIdAsync(Guid tipoId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipoId", tipoId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE tipo_id = @tipoId " +
                "ORDER BY planta_nombre";

            var resultadoPlantas = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPlantas];
        }


        public async Task<Planta> GetByIdAsync(Guid plantaId)
        {
            Planta unaPlanta = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@plantaId", plantaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE planta_id = @plantaId";

            var resultado = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaPlanta = resultado.First();

            return unaPlanta;
        }

        public async Task<Planta> GetByDetailsAsync(string planta_nombre, Guid ubicacionId, Guid tipoId)
        {
            Planta unaPlanta = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@planta_nombre", planta_nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@ubicacionId", ubicacionId,
                                    DbType.Guid, ParameterDirection.Input);
            parametrosSentencia.Add("@tipoId", tipoId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE LOWER(planta_nombre) = LOWER(@planta_nombre) " +
                "AND ubicacion_id = @ubicacionId " +
                "AND tipo_id = @tipoId";

            var resultado = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaPlanta = resultado.First();

            return unaPlanta;
        }

        public async Task<bool> CreateAsync(Planta unaPlanta)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_planta";
                var parametros = new
                {
                    p_nombre = unaPlanta.Nombre,
                    p_tipo_id = unaPlanta.TipoId,
                    p_ubicacion_id = unaPlanta.UbicacionId,
                    p_capacidad = unaPlanta.Capacidad
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

        public async Task<bool> UpdateAsync(Planta unaPlanta)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_planta";
                var parametros = new
                {
                    p_id = unaPlanta.Id,
                    p_nombre = unaPlanta.Nombre,
                    p_tipo_id = unaPlanta.TipoId,
                    p_ubicacion_id = unaPlanta.UbicacionId,
                    p_capacidad = unaPlanta.Capacidad
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

        public async Task<bool> RemoveAsync(Guid plantaId)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_planta";
                var parametros = new
                {
                    p_id = plantaId
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
