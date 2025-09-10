using Dapper;
using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
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
                "id, plantaId plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
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
                "id, plantaId plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE plantaId = @plantaId " +
                "ORDER BY fecha";

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
                "id, plantaId plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
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
                "id, plantaId plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE id = @eventoId ";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unEvento = resultado.First();

            return unEvento;
        }
    }
}
