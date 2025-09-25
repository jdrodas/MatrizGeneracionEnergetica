using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("tipos")]
        public long Tipos { get; set; } = 0;

        [JsonPropertyName("plantas")]
        public long Plantas { get; set; } = 0;

        [JsonPropertyName("eventos")]
        public long Eventos { get; set; } = 0;

        [JsonPropertyName("municipios")]
        public long Municipios { get; set; } = 0;

        [JsonPropertyName("departamentos")]
        public long Departamentos { get; set; } = 0;
    }
}