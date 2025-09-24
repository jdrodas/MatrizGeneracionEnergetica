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


-- Colección: produccion
db.createCollection("produccion",{
        validator:{
            $jsonSchema:{
                bsonType: 'object',
                title: 'Los eventos de producción de las plantas de generación',
                required: [
                    "_id",
                    "planta_id",
                    "planta_nombre",
                    "valor",
                    "fecha"
                ],
                properties:{
                    _id: {
                        bsonType: 'objectId'
                    },                    
                    planta_id: {
                        bsonType: ["objectId","string"],
                        description: "Id del tipo de la planta de generación",
                        minLength: 3
                    },                    
                    planta_nombre: {
                        bsonType: 'string',
                        description: "nombre del tipo de la planta de  geneneración",
                        minLength: 3
                    },                    
                    valor: {
                        bsonType:  ["double","int"],
                        description: "Valor en MW del evento de generación de la planta",
                        minimum: 0
                    },                    
                    fecha: {
                        bsonType: 'string',
                        description: "fecha en formato YYYY-MM-DD del evento de generación de la planta",
                        minLength: 3
                    }
                }
            }
        }
    }
);



-- ****************************************
--   Creación de Vistas
-- ****************************************

db.createView("v_info_plantas", "plantas", [
  {
    $lookup: {
      from: "tipos",
      localField: "tipo_id",
      foreignField: "_id",
      as: "tipo_info"
    }
  },
  {
    $lookup: {
      from: "ubicaciones",
      localField: "ubicacion_id",
      foreignField: "_id",
      as: "ubicacion_info"
    }
  },
  {
    $unwind: "$tipo_info"
  },
  {
    $unwind: "$ubicacion_info"
  },
  {
    $project: {
      _id: 1,
      nombre: 1,
      capacidad: 1,
      tipo_id: 1,
      tipo_nombre: "$tipo_info.nombre",
      esRenovable: "$tipo_info.esRenovable",
      ubicacion_id: 1,
      iso_departamento: "$ubicacion_info.iso_departamento",
      ubicacion_concatenada: {
        $concat: [
          "$ubicacion_info.nombre_municipio",
          ", ",
          "$ubicacion_info.nombre_departamento"
        ]
      }
    }
  }
]);

db.createView("v_produccion_tipo_dia", "produccion", [
  {
    $lookup: {
      from: "plantas",
      localField: "planta_id",
      foreignField: "_id",
      as: "planta_info"
    }
  },
  {
    $unwind: "$planta_info"
  },
  {
    $lookup: {
      from: "tipos",
      localField: "planta_info.tipo_id",
      foreignField: "_id",
      as: "tipo_info"
    }
  },
  {
    $unwind: "$tipo_info"
  },
  {
    $group: {
      _id: {
        tipo_id: "$tipo_info._id",
        fecha: "$fecha"
      },
      nombre_tipo: { $first: "$tipo_info.nombre" },
      esRenovable: { $first: "$tipo_info.esRenovable" },
      suma_valor: { $sum: "$valor" }
    }
  },
  {
    $project: {
      _id: 0,
      tipo_id: "$_id.tipo_id",
      nombre_tipo: 1,
      esRenovable: 1,
      fecha: "$_id.fecha",
      suma_valor: 1
    }
  },
  {
    $sort: {
      fecha: 1,
      nombre_tipo: 1
    }
  }
]);