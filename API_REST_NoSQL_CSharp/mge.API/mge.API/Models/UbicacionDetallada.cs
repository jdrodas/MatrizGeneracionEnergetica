using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class UbicacionDetallada : Ubicacion
    {
        [JsonPropertyName("plantas")]
        public List<Planta>? Plantas { get; set; } = null;

        [JsonPropertyName("totalPlantas")]
        public long TotalPlantas { get; set; } = 0;
    }
}
