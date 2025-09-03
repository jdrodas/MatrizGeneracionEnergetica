using Dapper;
using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using System.Data;

namespace mge.API.Repositories
{
    public class UbicacionRepository(PgsqlDbContext unContexto) : IUbicacionRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Ubicacion>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, codigo_departamento codigoDepartamento, iso_departamento isoDepartamento, " +
                "nombre_departamento nombreDepartamento, codigo_municipio codigoMunicipio, " +
                "nombre_municipio nombreMunicipio " +
                "FROM core.ubicaciones " +
                "ORDER BY codigo_departamento, nombre_departamento";

            var resultadoTipos = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoTipos];
        }

        public async Task<Ubicacion> GetByIdAsync(Guid ubicacion_id)
        {
            Ubicacion unTipo = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, codigo_departamento codigoDepartamento, iso_departamento isoDepartamento, " +
                "nombre_departamento nombreDepartamento, codigo_municipio codigoMunicipio, " +
                "nombre_municipio nombreMunicipio " +
                "FROM core.ubicaciones " +
                "WHERE id = @ubicacion_id";

            var resultado = await conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unTipo = resultado.First();

            return unTipo;
        }
    }
}
