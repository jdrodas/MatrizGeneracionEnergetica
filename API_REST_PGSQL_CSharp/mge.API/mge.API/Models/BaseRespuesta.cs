using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class BaseRespuesta
    {
        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; } = string.Empty;

        [JsonPropertyName("totalElementos")]
        public int TotalElementos { get; set; }

        [JsonPropertyName("pagina")]
        public int Pagina { get; set; }

        [JsonPropertyName("elementosPorPagina")]
        public int ElementosPorPagina { get; set; } // PageSize

        [JsonPropertyName("totalPaginas")]
        public int TotalPaginas { get; set; }
    }
}