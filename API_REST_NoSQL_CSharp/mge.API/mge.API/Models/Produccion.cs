using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Produccion
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("plantaId")]
        [BsonElement("planta_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? PlantaId { get; set; } = string.Empty;

        [JsonPropertyName("plantaNombre")]
        [BsonElement("planta_nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? PlantaNombre { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        [BsonElement("fecha")]
        [BsonRepresentation(BsonType.String)]
        public string? Fecha { get; set; } = string.Empty;

        [JsonPropertyName("valor")]
        [BsonElement("valor")]
        [BsonRepresentation(BsonType.Double)]
        public double Valor { get; set; } = 0.0d;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraProduccion = (Produccion)obj;

            return Id! == otraProduccion.Id
                && Valor!.Equals(otraProduccion.Valor)
                && PlantaId!.Equals(otraProduccion.PlantaId)
                && Fecha!.Equals(otraProduccion.Fecha);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + Valor.GetHashCode();
                hash = hash * 5 + (PlantaId?.GetHashCode() ?? 0);
                hash = hash * 5 + (Fecha?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
