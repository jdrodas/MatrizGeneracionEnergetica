using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.Repositories
{
    public class PlantaRepository(MongoDbContext unContexto) : IPlantaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Planta>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var lasPlantas = await coleccionPlantas
                .Find(_ => true)
                .SortBy(planta => planta.Nombre)
                .ToListAsync();

            return lasPlantas;
        }

        public async Task<List<Planta>> GetAllByLocationIdAsync(string ubicacionId)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var lasPlantas = await coleccionPlantas
                .Find(planta => planta.UbicacionId == ubicacionId)
                .SortBy(Planta => Planta.Nombre)
                .ToListAsync();

            return lasPlantas;
        }

        public async Task<List<Planta>> GetAllByTypeIdAsync(string tipoId)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var lasPlantas = await coleccionPlantas
                .Find(planta => planta.TipoId == tipoId)
                .SortBy(Planta => Planta.Nombre)
                .ToListAsync();

            return lasPlantas;
        }

        public async Task<Planta> GetByIdAsync(string plantaId)
        {
            Planta unaPlanta = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var resultado = await coleccionPlantas
                .Find(planta => planta.Id == plantaId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaPlanta = resultado;

            return unaPlanta;
        }

        public async Task<Planta> GetByNameAsync(string plantaNombre)
        {
            Planta unaPlanta = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var resultado = await coleccionPlantas
                .Find(planta => planta.Nombre!.ToLower() == plantaNombre.ToLower())
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaPlanta = resultado;

            return unaPlanta;
        }

        public async Task<Planta> GetByDetailsAsync(string plantaNombre, string plantaId)
        {
            Planta unaPlanta = new();

            if (!string.IsNullOrEmpty(plantaId))
                return await GetByIdAsync(plantaId);

            if (!string.IsNullOrEmpty(plantaNombre))
                return await GetByNameAsync(plantaNombre);

            return unaPlanta;
        }

        public async Task<Planta> GetByDetailsAsync(string plantaNombre, string ubicacionId, string tipoId)
        {
            Planta unaPlanta = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var builder = Builders<Planta>.Filter;
            var filtro = builder.And(
                builder.Eq(planta => planta.Nombre!.ToLower(), plantaNombre.ToLower()),
                builder.Eq(planta => planta.UbicacionId, ubicacionId),
                builder.Eq(planta => planta.TipoId, tipoId));

            var resultado = await coleccionPlantas
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaPlanta = resultado;

            return unaPlanta;
        }

        //public async Task<bool> CreateAsync(Planta unaPlanta)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_inserta_planta";
        //        var parametros = new
        //        {
        //            p_nombre = unaPlanta.Nombre,
        //            p_tipo_id = unaPlanta.TipoId,
        //            p_ubicacion_id = unaPlanta.UbicacionId,
        //            p_capacidad = unaPlanta.Capacidad
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //public async Task<bool> UpdateAsync(Planta unaPlanta)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_actualiza_planta";
        //        var parametros = new
        //        {
        //            p_id = unaPlanta.Id,
        //            p_nombre = unaPlanta.Nombre,
        //            p_tipo_id = unaPlanta.TipoId,
        //            p_ubicacion_id = unaPlanta.UbicacionId,
        //            p_capacidad = unaPlanta.Capacidad
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}

        //public async Task<bool> RemoveAsync(Guid plantaId)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_elimina_planta";
        //        var parametros = new
        //        {
        //            p_id = plantaId
        //        };

        //        var cantidad_filas = await conexion.ExecuteAsync(
        //            procedimiento,
        //            parametros,
        //            commandType: CommandType.StoredProcedure);

        //        if (cantidad_filas != 0)
        //            resultadoAccion = true;
        //    }
        //    catch (NpgsqlException error)
        //    {
        //        throw new DbOperationException(error.Message);
        //    }

        //    return resultadoAccion;
        //}
    }
}
