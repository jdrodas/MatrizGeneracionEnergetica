using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.DbContexts
{
    public class MongoDbContext(IConfiguration unaConfiguracion)
    {
        private readonly MGEDatabaseSettings _MGEDatabaseSettings = new(unaConfiguracion);

        public IMongoDatabase CreateConnection()
        {
            var configuracion = unaConfiguracion.GetSection("MGEDatabaseSettings");

            var baseDeDatos = configuracion.GetSection("BaseDeDatos").Value!;
            var usuario = configuracion.GetSection("Usuario").Value!;
            var password = configuracion.GetSection("PassWord").Value!;
            var servidor = configuracion.GetSection("Servidor").Value!;
            var puerto = configuracion.GetSection("Puerto").Value!;
            var mecanismoAutenticacion = configuracion.GetSection("MecanismoAutenticacion").Value!;

            var cadenaConexion = $"mongodb://{usuario}:{password}@{servidor}:{puerto}/{baseDeDatos}?authMechanism={mecanismoAutenticacion}";

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase(_MGEDatabaseSettings.BaseDeDatos);

            return miDB;
        }

        public MGEDatabaseSettings ConfiguracionColecciones
        {
            get { return _MGEDatabaseSettings; }
        }
    }
}