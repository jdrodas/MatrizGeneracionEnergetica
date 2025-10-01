namespace mge.API.Models
{
    public class PlantaParametrosConsulta : BaseParametrosConsulta
    {
        private static new readonly List<string> criteriosValidos =
            ["nombre", "ubicacionNombre", "tipoNombre"];

        public string? UbicacionNombre { get; set; } = string.Empty;

        public string? TipoNombre { get; set; } = string.Empty;
    }
}