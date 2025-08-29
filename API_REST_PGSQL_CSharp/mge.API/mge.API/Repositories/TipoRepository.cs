using Dapper;
using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
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

            var resultado = await conexion.QueryAsync<Tipo>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unTipo = resultado.First();

            return unTipo;
        }
    }
}
