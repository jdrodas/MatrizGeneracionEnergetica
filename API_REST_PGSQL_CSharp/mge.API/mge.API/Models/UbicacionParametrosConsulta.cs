namespace mge.API.Models
{
    public class UbicacionParametrosConsulta : BaseParametrosConsulta
    {
        private static new readonly List<string> criteriosValidos =
            ["nombre", "deptoIso"];

        public string? DeptoIso { get; set; } = string.Empty;
    }
}