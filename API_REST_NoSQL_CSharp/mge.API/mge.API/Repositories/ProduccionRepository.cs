using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Driver;
using System.Globalization;

namespace mge.API.Repositories
{
    public class ProduccionRepository(MongoDbContext unContexto) : IProduccionRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Produccion>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var losEventos = await coleccionProduccion
                .Find(_ => true)
                .SortBy(evento => evento.PlantaNombre)
                .ToListAsync();

            ModificaFormatoFechas(losEventos);

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

            ModificaFormatoFechas(losEventos);

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

            ModificaFormatoFechas(losEventos);

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

            ModificaFormatoFechas(unEvento);

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
            {
                ModificaFormatoFechas(resultado);
                eventoExistente = resultado;
            }
            return eventoExistente;
        }

        public async Task<bool> CreateAsync(Produccion unEvento)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            await coleccionProduccion
                .InsertOneAsync(unEvento);

            var resultado = await GetByDetailsAsync(unEvento);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Produccion unEvento)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var resultado = await coleccionProduccion
                .ReplaceOneAsync(evento => evento.Id == unEvento.Id, unEvento);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string eventoId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionProduccion = conexion
                .GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);

            var resultado = await coleccionProduccion
                .DeleteOneAsync(evento => evento.Id == eventoId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        private static void ModificaFormatoFechas(List<Produccion> losEventos)
        {
            DateTime fechaEvento;

            foreach (Produccion unEvento in losEventos)
            {
                fechaEvento = DateTime.ParseExact(unEvento.Fecha!, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                unEvento.Fecha = fechaEvento.ToString("dd-MM-yyyy");
            }
        }

        private static void ModificaFormatoFechas(Produccion unEvento)
        {
            DateTime fechaEvento = DateTime.ParseExact(unEvento.Fecha!, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            unEvento.Fecha = fechaEvento.ToString("dd-MM-yyyy");
        }
    }
}
