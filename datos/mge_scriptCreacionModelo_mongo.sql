-- Scripts de clase - Septiembre 19 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: MGE - Matriz de Generación Energética
-- Motor de Base de datos: MongoDB 8.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull mongodb/mongodb-community-server

-- Crear el contenedor
docker run --name mongodb_mge -e “MONGO_INITDB_ROOT_USERNAME=mongoadmin” -e MONGO_INITDB_ROOT_PASSWORD=unaClav3 -p 27017:27017 -d mongodb/mongodb-community-server:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Para conectarse al contenedor
mongodb://mongoadmin:unaClav3@localhost:27017/

-- Con usuario mongoadmin:

-- Para saber que versión de Mongo se está usando
db.version()

-- crear la base de datos
use mge_db;

-- Crear el rol para el usuario de gestion de Documentos en las colecciones
db.createRole(
  {
    role: "GestorDocumentos",
    privileges: [
        {
            resource: { 
                db: "mge_db", 
                collection: "" 
            }, 
            actions: [
                "find", 
                "insert", 
                "update", 
                "remove",
                "listCollections"
            ]
        }
    ],
    roles: []
  }
);

-- Crear usuario para gestionar el modelo

db.createUser({
  user: "mge_app",
  pwd: "unaClav3",  
  roles: [
    { role: "readWrite", db: "mge_db" },
    { role: "dbAdmin", db: "mge_db" }
  ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

db.createUser(
  {
    user: "mge_usr",
    pwd: "unaClav3",
    roles: [ 
    { role: "GestorDocumentos", db: "mge_db" }
    ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

-- Para saber que usuarios hay creados en la base de datos
db.getUsers()

-- Con el usuario mge_db

-- ****************************************
--   Creación de Colecciones
-- ****************************************

-- Colección: tipos
db.createCollection("tipos",{
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                title: 'Los tipos de fuentes de energía para la generación',
                required: [                    
                    "_id",
                    "nombre",
                    "descripcion",
                    "esRenovable"
                ],
                properties: {
                    _id: {
                        bsonType: 'objectId'
                    },
                    nombre: {
                        bsonType: 'string',
                        description: "'nombre' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 3
                    },
                    descripcion: {
                        bsonType: 'string',
                        description: "'descripcion' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 1
                    },
                    esRenovable: {
                        bsonType: 'bool',
                        description: "'esRenovable' debe ser un valor booleano y no puede ser nulo"                        
                    }
                }
            }
        }
    } 
);

db.createCollection("ubicaciones",{
        validator:{
            $jsonSchema:{
                bsonType: 'object',
                title: 'Las ubicaciones geográficas donde están las plantas de generación',
                required: [
                    "_id",
                    "codigo_departamento",
                    "codigo_municipio",
                    "nombre_departamento",
                    "nombre_municipio",
                    "iso_departamento"
                ],
                properties:{
                    _id: {
                        bsonType: 'objectId'
                    },
                    codigo_departamento: {
                        bsonType: 'string',
                        description: 'Código DANE DIVIPOLA del departamento',
                        maxLength: 2
                    },
                    codigo_municipio: {
                        bsonType: 'string',
                        description: 'Código DANE DIVIPOLA del municipio',
                        maxLength: 8
                    },                    
                    nombre_departamento: {
                        bsonType: 'string',
                        description: "'nombre_departamento' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 3
                    },
                    nombre_municipio: {
                        bsonType: 'string',
                        description: "'nombre_municipio' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 3
                    },
                    iso_departamento: {
                        bsonType: 'string',
                        description: "'iso_departamento' debe ser una cadena de caracteres y no puede ser nulo",
                        maxLength: 6
                    }
                }
            }
        }
    }
);

-- Colección: plantas
db.createCollection("plantas",{
        validator:{
            $jsonSchema:{
                bsonType: 'object',
                title: 'Las plantas de generación de energía eléctrica',
                required: [
                    "_id",
                    "nombre",
                    "tipo_id",
                    "tipo_nombre",
                    "ubicacion_id",
                    "ubicacion_nombre",
                    "capacidad"
                ],
                properties:{
                    _id: {
                        bsonType: 'objectId'
                    },                    
                    nombre: {
                        bsonType: 'string',
                        description: "'nombre' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 3
                    },                    
                    tipo_id: {
                        bsonType: ["objectId","string"],
                        description: "Id del tipo de fuente de generación",
                        minLength: 3
                    },                    
                    tipo_nombre: {
                        bsonType: 'string',
                        description: "nombre del tipo de fuente de geneneración",
                        minLength: 3
                    },                    
                    ubicacion_id: {
                        bsonType: ["objectId","string"],
                        description: "Id de la ubicación geográfica donde está la planta",
                        minLength: 3
                    },                    
                    ubicacion_nombre: {
                        bsonType: 'string',
                        description: "descripción de la ubicación en formato (municipio, departamento)",
                        minLength: 3
                    },                    
                    capacidad: {
                        bsonType:  ["double","int"],
                        description: "Capacidad de generación en Megavatios de la planta",
                        minimum: 0
                    }
                }
            }
        }
    }
);
