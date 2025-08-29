/*
AppValidationException:
Excepcion creada para enviar mensajes relacionados 
con la validación de datos en las capas de servicio
*/

namespace mge.API.Exceptions
{
    public class AppValidationException(string message) : Exception(message)
    {
    }
}