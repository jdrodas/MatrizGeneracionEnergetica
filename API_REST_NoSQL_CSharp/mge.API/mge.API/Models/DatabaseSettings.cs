namespace mge.API.Models
{
    public class DatabaseSettings
    {
        public string Database { get; set; } = null!;
        public string ColeccionTipos { get; set; } = null!;
        public string ColeccionPlantas { get; set; } = null!;
        public string ColeccionUbicaciones { get; set; } = null!;
        public string ColeccionProduccion { get; set; } = null!;

        public DatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("DatabaseSettings");

            Database = configuracion.GetSection("Database").Value!;
            ColeccionTipos = configuracion.GetSection("TypesCollection").Value!;
            ColeccionPlantas = configuracion.GetSection("PlantsCollection").Value!;
            ColeccionUbicaciones = configuracion.GetSection("LocationsCollection").Value!;
            ColeccionProduccion = configuracion.GetSection("ProductionCollection").Value!;
        }
    }
}
