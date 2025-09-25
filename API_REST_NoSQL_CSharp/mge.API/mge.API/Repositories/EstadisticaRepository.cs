using mge.API.DbContexts;
using mge.API.Interfaces;
using mge.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mge.API.Repositories
{
    public class EstadisticaRepository(MongoDbContext unContexto) : IEstadisticaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            Estadistica unaEstadistica = new();
            var conexion = contextoDB.CreateConnection();

            //Total Tipos
            var coleccionTipos = conexion.GetCollection<Tipo>(contextoDB.ConfiguracionColecciones.ColeccionTipos);
            var totalTipos = await coleccionTipos
                .EstimatedDocumentCountAsync();

            unaEstadistica.Tipos = totalTipos;

            //Total Plantas
            var coleccionPlantas = conexion.GetCollection<Planta>(contextoDB.ConfiguracionColecciones.ColeccionPlantas);
            var totalPlantas = await coleccionPlantas
                .EstimatedDocumentCountAsync();

            unaEstadistica.Plantas = totalPlantas;

            //Total Eventos Producción
            var coleccionProduccion = conexion.GetCollection<Produccion>(contextoDB.ConfiguracionColecciones.ColeccionProduccion);
            var totalEventos = await coleccionProduccion
                .EstimatedDocumentCountAsync();

            unaEstadistica.Eventos = totalEventos;

            //Total Municipios
            var coleccionUbicaciones = conexion.GetCollection<Ubicacion>(contextoDB.ConfiguracionColecciones.ColeccionUbicaciones);
            var totalMunicipios = await coleccionUbicaciones
                .EstimatedDocumentCountAsync();

            unaEstadistica.Municipios = totalMunicipios;

            //Total Departamentos
            var filtroUbicaciones = Builders<Ubicacion>.Filter.Empty;
            var departamentos = await coleccionUbicaciones
                .DistinctAsync<string>("codigo_departamento", filtroUbicaciones)
                .Result
                .ToListAsync();

            unaEstadistica.Departamentos = departamentos.Count;

            return unaEstadistica;
        }
    }
}