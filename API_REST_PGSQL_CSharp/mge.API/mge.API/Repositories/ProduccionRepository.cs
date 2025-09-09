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
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "ORDER BY fecha";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoProduccion];
        }

        public async Task<List<Produccion>> GetAllByPlantIdAsync(Guid planta_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@planta_id", planta_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE planta_id = @planta_id " +
                "ORDER BY fecha";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoProduccion];
        }

        public async Task<List<Produccion>> GetAllByDateIdAsync(string fecha_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fecha_id", fecha_id,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE to_char(fecha,'DD-MM-YYYY') = @fecha_id " +
                "ORDER BY fecha";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoProduccion];
        }

        public async Task<Produccion> GetByIdAsync(Guid evento_id)
        {
            Produccion unEvento = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@evento_id", evento_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, to_char(fecha,'DD-MM-YYYY') fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE id = @evento_id ";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unEvento = resultado.First();

            return unEvento;
        }
    }
}
