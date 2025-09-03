using System.Text.Json.Serialization;

namespace mge.API.Models
{    
    public class Planta
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("capacidad")]
        public double Capacidad { get; set; } = 0.0d;

        [JsonPropertyName("ubicacionId")]
        public Guid UbicacionId { get; set; } = Guid.Empty;

        [JsonPropertyName("ubicacionNombre")]
        public string? UbicacionNombre { get; set; } = string.Empty;

        [JsonPropertyName("tipoId")]
        public Guid TipoId { get; set; } = Guid.Empty;

        [JsonPropertyName("tipoNombre")]
        public string? TipoNombre { get; set; } = string.Empty;
    }
}
