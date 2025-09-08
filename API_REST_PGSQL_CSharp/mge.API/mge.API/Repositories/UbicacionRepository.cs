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

            var resultado = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unTipo = resultado.First();

            return unTipo;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacion_nombre)
        {
            Ubicacion unTipo = new();
            var conexion = contextoDB.CreateConnection();

            string[] datosUbicacion = ubicacion_nombre.Split(',');

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre_municipio", datosUbicacion[0].Trim(),
                                    DbType.String, ParameterDirection.Input);

            parametrosSentencia.Add("@nombre_departamento", datosUbicacion[1].Trim(),
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, codigo_departamento codigoDepartamento, iso_departamento isoDepartamento, " +
                "nombre_departamento nombreDepartamento, codigo_municipio codigoMunicipio, " +
                "nombre_municipio nombreMunicipio " +
                "FROM core.ubicaciones " +
                "WHERE LOWER(nombre_municipio) = LOWER(@nombre_municipio) " +
                "AND  LOWER(nombre_departamento) = LOWER(@nombre_departamento)";

            var resultado = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unTipo = resultado.First();

            return unTipo;
        }

        public async Task<Ubicacion> GetByDetailsAsync(Guid ubicacion_id, string ubicacion_nombre)
        {
            Ubicacion ubicacionExistente = new();

            if (string.IsNullOrEmpty(ubicacion_nombre) && ubicacion_id == Guid.Empty)
                throw new AppValidationException("Datos insuficientes para obtener la ubiación");

            if (!string.IsNullOrEmpty(ubicacion_nombre) && ubicacion_id == Guid.Empty)
                ubicacionExistente = await GetByNameAsync(ubicacion_nombre);

            if (ubicacion_id != Guid.Empty)
                ubicacionExistente = await GetByIdAsync(ubicacion_id);

            return ubicacionExistente;
        }
    }
}
