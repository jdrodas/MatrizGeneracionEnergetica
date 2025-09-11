using Dapper;
using mge.API.DbContexts;
using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace mge.API.Repositories
{
    public class ProduccionRepository(PgsqlDbContext unContexto) : IProduccionRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Produccion>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "ORDER BY fecha";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoProduccion];
        }

        public async Task<List<Produccion>> GetAllByPlantIdAsync(Guid plantaId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@plantaId", plantaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE planta_id = @plantaId ";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoProduccion];
        }

        public async Task<List<Produccion>> GetAllByDateIdAsync(string fechaId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fechaId", fechaId,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE to_char(fecha,'DD-MM-YYYY') = @fechaId " +
                "ORDER BY fecha";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoProduccion];
        }

        public async Task<Produccion> GetByIdAsync(Guid eventoId)
        {
            Produccion unEvento = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@eventoId", eventoId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE id = @eventoId ";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unEvento = resultado.First();

            return unEvento;
        }

        public async Task<Produccion> GetByDetailsAsync(Produccion unEvento)
        {
            Produccion eventoExistente = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@plantaId", unEvento.PlantaId,
                                    DbType.Guid, ParameterDirection.Input);
            parametrosSentencia.Add("@valor", unEvento.Valor,
                                    DbType.Double, ParameterDirection.Input);
            parametrosSentencia.Add("@fecha", unEvento.Fecha,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE planta_id = @plantaId " +
                "AND to_char(fecha,'DD-MM-YYYY') = @fecha " +
                "AND valor = @valor";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                eventoExistente = resultado.First();

            return eventoExistente;
        }

        public async Task<bool> CreateAsync(Produccion unEvento)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_produccion";
                var parametros = new
                {
                    p_planta_id = unEvento.PlantaId,
                    p_fecha     = unEvento.Fecha,
                    p_valor     = unEvento.Valor
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
