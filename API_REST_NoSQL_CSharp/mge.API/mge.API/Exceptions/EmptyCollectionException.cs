/*
EmptyCollectionException:
Excepcion creada para enviar mensajes relacionados 
con las colecciones o respuestas vacías
*/

namespace mge.API.Exceptions
{
    public class EmptyCollectionException(string message) : Exception(message)
    {
    }
}