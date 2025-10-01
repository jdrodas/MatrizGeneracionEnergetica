namespace mge.API.Models
{
    public class ProduccionParametrosConsulta : BaseParametrosConsulta
    {
        private static new readonly List<string> criteriosValidos =
            ["fecha", "plantaNombre"];

        public string? PlantaNombre { get; set; } = string.Empty;

        public string? Fecha { get; set; } = string.Empty;
    }
}