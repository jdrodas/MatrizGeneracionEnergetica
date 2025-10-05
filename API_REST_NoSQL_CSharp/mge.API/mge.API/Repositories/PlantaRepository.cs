using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.Repositories
{
    public class PlantaRepository(MongoDbContext unContexto) : IPlantaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Planta>> GetAllAsync()
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
                builder.Regex(planta => planta.Nombre, $"/^{plantaNombre}$/i"),
                builder.Eq(planta => planta.UbicacionId, ubicacionId),
                builder.Eq(planta => planta.TipoId, tipoId));

            var resultado = await coleccionPlantas
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaPlanta = resultado;

            return unaPlanta;
        }

        public async Task<bool> CreateAsync(Planta unaPlanta)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            await coleccionPlantas
                .InsertOneAsync(unaPlanta);

            var resultado = await GetByDetailsAsync(unaPlanta.Nombre!, unaPlanta.UbicacionId!, unaPlanta.TipoId!);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Planta unaPlanta)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var resultado = await coleccionPlantas
                .ReplaceOneAsync(planta => planta.Id == unaPlanta.Id, unaPlanta);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateSourceTypeAsync(Tipo unTipo)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();
            
            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var filtroPlantas = Builders<Planta>
                .Filter
                .Eq(planta => planta.TipoId, unTipo.Id);

            var actualizadorPlantas = Builders<Planta>
                .Update
                .Set(planta => planta.TipoNombre, unTipo.Nombre);

            var resultado = await coleccionPlantas
                .UpdateManyAsync(filtroPlantas, actualizadorPlantas);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string plantaId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPlantas = conexion
                .GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);

            var resultado = await coleccionPlantas
                .DeleteOneAsync(planta => planta.Id == plantaId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
