using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
namespace mge.API.Models
{
    public class Tipo
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        [BsonElement("descripcion")]
        [BsonRepresentation(BsonType.String)]
        public string? Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("esRenovable")]
        [BsonElement("esRenovable")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool EsRenovable { get; set; } = false;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroTipo = (Tipo)obj;

            return Id == otroTipo.Id
                && Nombre!.Equals(otroTipo.Nombre)
                && Descripcion!.Equals(otroTipo.Descripcion)
                && EsRenovable.Equals(otroTipo.EsRenovable);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Descripcion?.GetHashCode() ?? 0);
                hash = hash * 5 + EsRenovable.GetHashCode();

                return hash;
            }
        }
    }
}