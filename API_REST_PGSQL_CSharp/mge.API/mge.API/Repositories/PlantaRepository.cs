using Dapper;
using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
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

        public async Task<List<Planta>> GetAllByLocationIdAsync(Guid ubicacion_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE ubicacion_id = @ubicacion_id " +
                "ORDER BY planta_nombre";

            var resultadoPlantas = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPlantas];
        }

        public async Task<List<Planta>> GetAllByTypeIdAsync(Guid tipo_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipo_id", tipo_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE tipo_id = @tipo_id " +
                "ORDER BY planta_nombre";

            var resultadoPlantas = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPlantas];
        }

        public async Task<List<Planta>> GetAllByDeptoIsoAsync(string depto_iso)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@depto_iso", depto_iso,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE LOWER(iso_departamento) = LOWER(@depto_iso) " +
                "ORDER BY planta_nombre";

            var resultadoPlantas = await conexion
                .QueryAsync<Planta>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPlantas];
        }


        public async Task<Planta> GetByIdAsync(Guid planta_id)
        {
            Planta unaPlanta = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@planta_id", planta_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT " +
                "planta_id id, planta_nombre nombre, capacidad, " +
                "ubicacion_id ubicacionId, ubicacion_nombre ubicacionNombre, " +
                "tipo_id tipoId, tipo_nombre tipoNombre " +
                "FROM core.v_info_plantas " +
                "WHERE planta_id = @planta_id";

            var resultado = await conexion.QueryAsync<Planta>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaPlanta = resultado.First();

            return unaPlanta;
        }
    }
}
