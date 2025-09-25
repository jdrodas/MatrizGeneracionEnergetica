using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Planta
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("capacidad")]
        public double Capacidad { get; set; } = 0.0d;

        [JsonPropertyName("ubicacionId")]
        public Guid UbicacionId { get; set; } = Guid.Empty;

        [JsonPropertyName("ubicacionNombre")]
        public string? UbicacionNombre { get; set; } = string.Empty;

        [JsonPropertyName("tipoId")]
        public Guid TipoId { get; set; } = Guid.Empty;

        [JsonPropertyName("tipoNombre")]
        public string? TipoNombre { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraPlanta = (Planta)obj;

            return Id == otraPlanta.Id
                && Nombre!.Equals(otraPlanta.Nombre)
                && Capacidad!.Equals(otraPlanta.Capacidad)
                && TipoId.Equals(otraPlanta.TipoId)
                && UbicacionId.Equals(otraPlanta.UbicacionId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + Capacidad.GetHashCode();
                hash = hash * 5 + TipoId.GetHashCode(); ;
                hash = hash * 5 + UbicacionId.GetHashCode();

                return hash;
            }
        }
    }
}
