using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class PlantaRespuesta : BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Planta> Data { get; set; } = [];
    }
}