using Dapper;
using mge.API.DbContexts;
using mge.API.Exceptions;
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

            var resultadoUbicaciones = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoUbicaciones];
        }

        public async Task<List<Ubicacion>> GetAllByDeptoIsoAsync(string deptoIso)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@deptoIso", deptoIso,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, codigo_departamento codigoDepartamento, iso_departamento isoDepartamento, " +
                "nombre_departamento nombreDepartamento, codigo_municipio codigoMunicipio, " +
                "nombre_municipio nombreMunicipio " +
                "FROM core.ubicaciones " +
                "WHERE LOWER(iso_departamento) = LOWER(@deptoIso) " +
                "ORDER BY nombre_municipio";

            var resultadoUbicaciones = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoUbicaciones];
        }

        public async Task<Ubicacion> GetByIdAsync(Guid ubicacionId)
        {
            Ubicacion unaUbicacion = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacionId", ubicacionId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, codigo_departamento codigoDepartamento, iso_departamento isoDepartamento, " +
                "nombre_departamento nombreDepartamento, codigo_municipio codigoMunicipio, " +
                "nombre_municipio nombreMunicipio " +
                "FROM core.ubicaciones " +
                "WHERE id = @ubicacionId";

            var resultado = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacionNombre)
        {
            Ubicacion unaUbicacion = new();
            var conexion = contextoDB.CreateConnection();

            string[] datosUbicacion = ubicacionNombre.Split(',');

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombreMunicipio", datosUbicacion[0].Trim(),
                                    DbType.String, ParameterDirection.Input);

            parametrosSentencia.Add("@nombreDepartamento", datosUbicacion[1].Trim(),
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, codigo_departamento codigoDepartamento, iso_departamento isoDepartamento, " +
                "nombre_departamento nombreDepartamento, codigo_municipio codigoMunicipio, " +
                "nombre_municipio nombreMunicipio " +
                "FROM core.ubicaciones " +
                "WHERE LOWER(nombre_municipio) = LOWER(@nombreMunicipio) " +
                "AND  LOWER(nombre_departamento) = LOWER(@nombreDepartamento)";

            var resultado = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByDetailsAsync(Guid ubicacionId, string ubicacionNombre)
        {
            Ubicacion ubicacionExistente = new();

            if (string.IsNullOrEmpty(ubicacionNombre) && ubicacionId == Guid.Empty)
                throw new AppValidationException("Datos insuficientes para obtener la ubicación");

            if (!string.IsNullOrEmpty(ubicacionNombre) && ubicacionId == Guid.Empty)
                ubicacionExistente = await GetByNameAsync(ubicacionNombre);

            if (ubicacionId != Guid.Empty)
                ubicacionExistente = await GetByIdAsync(ubicacionId);

            return ubicacionExistente;
        }
    }
}
