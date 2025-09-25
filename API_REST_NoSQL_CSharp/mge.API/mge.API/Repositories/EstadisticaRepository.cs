using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;

namespace mge.API.Repositories
{
    public class EstadisticaRepository(MongoDbContext unContexto) : IEstadisticaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            Estadistica unaEstadistica = new();
            var conexion = contextoDB.CreateConnection();

            //Total Periodos
            var coleccionTipos = conexion.GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);
            var totalTipos = await coleccionTipos
                .EstimatedDocumentCountAsync();

            unaEstadistica.Tipos = totalTipos;

            return unaEstadistica;
        }
    }
}