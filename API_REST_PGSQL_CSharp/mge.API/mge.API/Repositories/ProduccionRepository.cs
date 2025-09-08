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

        public async Task<List<Produccion>> GetAllByPlantIdAsync(Guid planta_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@planta_id", planta_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "id, planta_id plantaId, planta_nombre plantaNombre, valor, fecha " +
                "FROM core.v_info_produccion_planta " +
                "WHERE planta_id = @planta_id " +
                "ORDER BY fecha";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoProduccion];
        }
    }
}
