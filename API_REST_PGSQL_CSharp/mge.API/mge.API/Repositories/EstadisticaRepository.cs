using Dapper;
using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Repositories
{
    public class EstadisticaRepository(PgsqlDbContext unContexto) : IEstadisticaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            Estadistica conteoRegistros = new();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.tipos";

            conteoRegistros.TotalTipos = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.plantas";

            conteoRegistros.TotalPlantas = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.produccion";

            conteoRegistros.TotalEventos = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(DISTINCT codigo_departamento) total FROM core.ubicaciones;";

            conteoRegistros.TotalDepartamentos = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(DISTINCT codigo_municipio) total FROM core.ubicaciones;";

            conteoRegistros.TotalMunicipios = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            return conteoRegistros;
        }

    }
}
