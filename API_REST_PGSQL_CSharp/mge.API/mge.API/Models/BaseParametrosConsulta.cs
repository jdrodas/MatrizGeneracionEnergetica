namespace mge.API.Models
{
    public class BaseParametrosConsulta
    {
        //Definiciones para los criterios de consulta
        protected static readonly List<string> criteriosValidos = ["id", "nombre"];
        protected string criterio = string.Empty;

        public string Criterio
        {
            get
            {
                if (string.IsNullOrEmpty(criterio))
                    criterio = "nombre";

                return criterio;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !criteriosValidos.Contains(value.ToLower()))
                    criterio = "nombre";
                else
                    criterio = value;
            }
        }

        //Propiedades bases para implementar la consulta por criterios

        public Guid Id { get; set; } = Guid.Empty;
        public string? Nombre { get; set; } = string.Empty;





    }
}