using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("totalTipos")]
        public long Tipos { get; set; } = 0;

        [JsonPropertyName("totalPlantas")]
        public long Plantas { get; set; } = 0;

        [JsonPropertyName("totalEventos")]
        public long Eventos { get; set; } = 0;

        [JsonPropertyName("totalMunicipios")]
        public long Municipios { get; set; } = 0;

        [JsonPropertyName("totalDepartamentos")]
        public long Departamentos { get; set; } = 0;
    }
}