using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class ProduccionRespuesta : BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Produccion> Data { get; set; } = [];
    }
}