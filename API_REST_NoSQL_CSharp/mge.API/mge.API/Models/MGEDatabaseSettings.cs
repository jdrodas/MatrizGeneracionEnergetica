namespace mge.API.Models
{
    public class MGEDatabaseSettings
    {
        public string BaseDeDatos { get; set; } = null!;
        public string ColeccionTipos { get; set; } = null!;
        public string ColeccionPlantas { get; set; } = null!;
        public string ColeccionUbicaciones { get; set; } = null!;
        public string ColeccionProduccion { get; set; } = null!;

        public MGEDatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("MGEDatabaseSettings");

            BaseDeDatos = configuracion.GetSection("BaseDeDatos").Value!;
            ColeccionTipos = configuracion.GetSection("ColeccionTipos").Value!;
            ColeccionPlantas = configuracion.GetSection("ColeccionPlantas").Value!;
            ColeccionUbicaciones = configuracion.GetSection("ColeccionUbicaciones").Value!;
            ColeccionProduccion = configuracion.GetSection("ColeccionProduccion").Value!;
        }
    }
}