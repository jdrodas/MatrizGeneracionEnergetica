using mge.API.Models;
using MongoDB.Driver;

namespace mge.API.DbContexts
{
    public class MongoDbContext(IConfiguration unaConfiguracion)
    {
        private readonly DatabaseSettings _databaseSettings = new(unaConfiguracion);

        public IMongoDatabase CreateConnection()
        {
            var configuracion = unaConfiguracion.GetSection("DatabaseSettings");

            var baseDeDatos = configuracion.GetSection("Database").Value!;
            var usuario = configuracion.GetSection("Username").Value!;
            var password = configuracion.GetSection("Password").Value!;
            var servidor = configuracion.GetSection("Host").Value!;
            var puerto = configuracion.GetSection("Port").Value!;
            var mecanismoAutenticacion = configuracion.GetSection("AuthMechanism").Value!;

            var cadenaConexion = $"mongodb://{usuario}:{password}@{servidor}:{puerto}/{baseDeDatos}?authMechanism={mecanismoAutenticacion}";

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase(baseDeDatos);

            return miDB;
        }

        public DatabaseSettings ConfiguracionColecciones
        {
            get { return _databaseSettings; }
        }
    }
}