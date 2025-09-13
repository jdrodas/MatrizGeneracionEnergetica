using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("totalTipos")]
        public long TotalTipos { get; set; } = 0;

        [JsonPropertyName("totalPlantas")]
        public long TotalPlantas { get; set; } = 0;

        [JsonPropertyName("totalEventos")]
        public long TotalEventos { get; set; } = 0;

        [JsonPropertyName("totalDepartamentos")]
        public long TotalDepartamentos { get; set; } = 0;

        [JsonPropertyName("totalMunicipios")]
        public long TotalMunicipios { get; set; } = 0;



    }
}
