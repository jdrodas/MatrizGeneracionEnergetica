using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class UbicacionRespuesta : BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Ubicacion> Data { get; set; } = [];
    }
}