using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Ubicacion
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("codigoDepartamento")]
        public string? CodigoDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("isoDepartamento")]
        public string? IsoDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("nombreDepartamento")]
        public string? NombreDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("codigoMunicipio")]
        public string? CodigoMunicipio { get; set; } = string.Empty;

        [JsonPropertyName("nombreMunicipio")]
        public string? NombreMunicipio { get; set; } = string.Empty;
    }
}
