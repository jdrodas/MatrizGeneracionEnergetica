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

        [JsonPropertyName("ubicacion_id")]
        public Guid UbicacionId { get; set; } = Guid.Empty;

        [JsonPropertyName("ubicacion_nombre")]
        public string? UbicacionNombre { get; set; } = string.Empty;

        [JsonPropertyName("tipo_id")]
        public Guid TipoId { get; set; } = Guid.Empty;

        [JsonPropertyName("tipo_nombre")]
        public string? TipoNombre { get; set; } = string.Empty;
    }
}
