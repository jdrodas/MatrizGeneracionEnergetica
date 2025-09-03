using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Tipo
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("esRenovable")]
        public bool EsRenovable { get; set; } = false;
    }
}
