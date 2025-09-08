using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Produccion
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("plantaId")]
        public Guid PlantaId { get; set; } = Guid.Empty;

        [JsonPropertyName("plantaNombre")]
        public string? PlantaNombre { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; } = new DateTime();

        [JsonPropertyName("valor")]
        public double Valor { get; set; } = 0.0d;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraProduccion = (Produccion)obj;

            return Id == otraProduccion.Id
                && Valor!.Equals(otraProduccion.Valor)
                && PlantaId.Equals(otraProduccion.PlantaId)
                && Fecha.Equals(otraProduccion.Fecha);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + Valor.GetHashCode();
                hash = hash * 5 + PlantaId.GetHashCode(); ;
                hash = hash * 5 + Fecha.GetHashCode();

                return hash;
            }
        }
    }
}
