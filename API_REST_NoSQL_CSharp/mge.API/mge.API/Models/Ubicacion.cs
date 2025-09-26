using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Ubicacion
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("codigoDepartamento")]
        [BsonElement("codigo_departamento")]
        [BsonRepresentation(BsonType.String)]
        public string? CodigoDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("isoDepartamento")]
        [BsonElement("iso_departamento")]
        [BsonRepresentation(BsonType.String)]
        public string? IsoDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("nombreDepartamento")]
        [BsonElement("nombre_departamento")]
        [BsonRepresentation(BsonType.String)]
        public string? NombreDepartamento { get; set; } = string.Empty;

        [JsonPropertyName("codigoMunicipio")]
        [BsonElement("codigo_municipio")]
        [BsonRepresentation(BsonType.String)]
        public string? CodigoMunicipio { get; set; } = string.Empty;

        [JsonPropertyName("nombreMunicipio")]
        [BsonElement("nombre_municipio")]
        [BsonRepresentation(BsonType.String)]
        public string? NombreMunicipio { get; set; } = string.Empty;
    }
}
