using mge.API.DbContexts;
using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.Repositories
{
    public class UbicacionRepository(MongoDbContext unContexto) : IUbicacionRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Ubicacion>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionUbicaciones = conexion
                .GetCollection<Ubicacion>(contextoDB.ConfiguracionColecciones.ColeccionUbicaciones);

            var lasUbicaciones = await coleccionUbicaciones
                .Find(_ => true)
                .SortBy(ubicacion => ubicacion.NombreMunicipio)
                .ToListAsync();

            return lasUbicaciones;
        }

        public async Task<List<Ubicacion>> GetAllByDeptoIsoAsync(string deptoIso)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionUbicaciones = conexion
                .GetCollection<Ubicacion>(contextoDB.ConfiguracionColecciones.ColeccionUbicaciones);

            var lasUbicaciones = await coleccionUbicaciones
                .Find(ubicacion => ubicacion.IsoDepartamento!.ToLower() == deptoIso.ToLower())
                .SortBy(ubicacion => ubicacion.NombreMunicipio)
                .ToListAsync();

            return lasUbicaciones;
        }

        public async Task<Ubicacion> GetByIdAsync(string tipoId)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionUbicaciones = conexion
                .GetCollection<Ubicacion>(contextoDB.ConfiguracionColecciones.ColeccionUbicaciones);

            var resultado = await coleccionUbicaciones
                .Find(tipo => tipo.Id == tipoId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByNameAsync(string ubicacionNombre)
        {
            Ubicacion unaUbicacion = new(); ;

            string[] datosUbicacion = ubicacionNombre.Split(',');

            var conexion = contextoDB
                .CreateConnection();

            var coleccionUbicaciones = conexion
                .GetCollection<Ubicacion>(contextoDB.ConfiguracionColecciones.ColeccionUbicaciones);

            var builder = Builders<Ubicacion>.Filter;
            var filtro = builder.And(
                builder.Eq(ubicacion => ubicacion.NombreMunicipio!.ToLower(), datosUbicacion[0].Trim().ToLower()),
                builder.Eq(ubicacion => ubicacion.NombreDepartamento!.ToLower(), datosUbicacion[1].Trim().ToLower()));

            var resultado = await coleccionUbicaciones
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaUbicacion = resultado;

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetByDetailsAsync(string ubicacionId, string ubicacionNombre)
        {
            Ubicacion ubicacionExistente = new();

            if (string.IsNullOrEmpty(ubicacionNombre) && string.IsNullOrEmpty(ubicacionId))
                throw new AppValidationException("Datos insuficientes para obtener la ubicación");

            if (!string.IsNullOrEmpty(ubicacionNombre) && ubicacionId == string.Empty)
                ubicacionExistente = await GetByNameAsync(ubicacionNombre);

            if (!string.IsNullOrEmpty(ubicacionId))
                ubicacionExistente = await GetByIdAsync(ubicacionId);

            return ubicacionExistente;
        }
    }
}
