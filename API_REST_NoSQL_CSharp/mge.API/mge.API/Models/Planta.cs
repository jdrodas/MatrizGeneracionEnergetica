using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Planta
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("capacidad")]
        [BsonElement("capacidad")]
        [BsonRepresentation(BsonType.Double)]
        public double Capacidad { get; set; } = 0.0d;

        [JsonPropertyName("ubicacionId")]
        [BsonElement("ubicacionId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UbicacionId { get; set; } = string.Empty;

        [JsonPropertyName("ubicacionNombre")]
        [BsonElement("ubicacionNombre")]
        [BsonRepresentation(BsonType.String)]
        public string? UbicacionNombre { get; set; } = string.Empty;

        [JsonPropertyName("tipoId")]
        [BsonElement("tipoId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TipoId { get; set; } = string.Empty;

        [JsonPropertyName("tipoNombre")]
        [BsonElement("tipoNombre")]
        [BsonRepresentation(BsonType.String)]
        public string? TipoNombre { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraPlanta = (Planta)obj;

            return Id == otraPlanta.Id
                && Nombre!.Equals(otraPlanta.Nombre)
                && Capacidad!.Equals(otraPlanta.Capacidad)
                && TipoId!.Equals(otraPlanta.TipoId)
                && UbicacionId!.Equals(otraPlanta.UbicacionId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + Capacidad.GetHashCode();
                hash = hash * 5 + (TipoId?.GetHashCode() ?? 0);
                hash = hash * 5 + (UbicacionId?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
