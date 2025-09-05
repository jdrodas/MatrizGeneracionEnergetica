using System.Text.Json.Serialization;

namespace mge.API.Models
{
    public class Tipo
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("esRenovable")]
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
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Descripcion?.GetHashCode() ?? 0);
                hash = hash * 5 + EsRenovable.GetHashCode();

                return hash;
            }
        }
    }
}
