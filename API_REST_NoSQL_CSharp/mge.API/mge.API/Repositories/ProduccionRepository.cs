using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.Repositories
{
    public class ProduccionRepository(MongoDbContext unContexto) : IProduccionRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Produccion>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var losEventos = await coleccionProduccion
                .Find(_ => true)
                .SortBy(evento => evento.PlantaNombre)
                .ToListAsync();

            return losEventos;
        }

        public async Task<List<Produccion>> GetAllByPlantIdAsync(string plantaId)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var losEventos = await coleccionProduccion
                .Find(evento => evento.PlantaId == plantaId)
                .SortBy(evento => evento.PlantaNombre)
                .ToListAsync();

            return losEventos;
        }

        public async Task<List<Produccion>> GetAllByDateIdAsync(string fechaId)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var losEventos = await coleccionProduccion
                .Find(evento => evento.Fecha == fechaId)
                .SortBy(evento => evento.PlantaNombre)
                .ToListAsync();

            return losEventos;
        }

        public async Task<Produccion> GetByIdAsync(string eventoId)
        {
            Produccion unEvento = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var resultado = await coleccionProduccion
                .Find(evento => evento.Id == eventoId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unEvento = resultado;

            return unEvento;
        }

        public async Task<Produccion> GetByDetailsAsync(Produccion unEvento)
        {
            Produccion eventoExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var builder = Builders<Produccion>.Filter;
            var filtro = builder.And(
                builder.Eq(evento => evento.PlantaId, unEvento.PlantaId),
                builder.Eq(evento => evento.Valor, unEvento.Valor),
                builder.Eq(evento => evento.Fecha, unEvento.Fecha));

            var resultado = await coleccionProduccion
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                eventoExistente = resultado;

            return eventoExistente;
        }

        //public async Task<bool> CreateAsync(Produccion unEvento)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_inserta_produccion";
        //        var parametros = new
        //        {
        //            p_planta_id = unEvento.PlantaId,
        //            p_fecha = unEvento.Fecha,
        //            p_valor = unEvento.Valor
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

        //public async Task<bool> UpdateAsync(Produccion unEvento)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_actualiza_produccion";
        //        var parametros = new
        //        {
        //            p_id = unEvento.Id,
        //            p_planta_id = unEvento.PlantaId,
        //            p_fecha = unEvento.Fecha,
        //            p_valor = unEvento.Valor
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

        //public async Task<bool> RemoveAsync(Guid eventoId)
        //{
        //    bool resultadoAccion = false;

        //    try
        //    {
        //        var conexion = contextoDB.CreateConnection();

        //        string procedimiento = "core.p_elimina_produccion";
        //        var parametros = new
        //        {
        //            p_id = eventoId
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
