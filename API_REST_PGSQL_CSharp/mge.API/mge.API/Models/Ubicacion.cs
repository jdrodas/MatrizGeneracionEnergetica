using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Ubicacion
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("codigo_departamento")]
        public string? CodigoDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("nombre_departamento")]
        public string? NombreDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("codigo_municipio")]
        public string? CodigoMunicipio { get; set; } = string.Empty;

        [JsonPropertyName("nombre_municipio")]
        public string? NombreMunicipio { get; set; } = string.Empty;
    }
}
