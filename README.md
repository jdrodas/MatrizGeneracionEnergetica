# Matriz Generación Energetica

Repositorio del proyecto de seguimiento a la generación energética por tipo de fuente.

API REST desarrollada como ejercicio demostrativo para el curso de **Tópicos Avanzados de Bases de Datos**, enfocada en la implementación del **patrón repositorio** y la separación por capas. El dominio de problema aborda el registro de composición de la matriz energética de un país, permitiendo el seguimiento de la producción diaria de diferentes fuentes de energía.

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/jdrodas/MatrizGeneracionEnergetica)
![License](https://img.shields.io/badge/license-Academic-orange)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![PostgreSQL](https://img.shields.io/badge/database-postgreSQL-blue)
![MongoDB](https://img.shields.io/badge/database-mongoDB-green)

## Objetivos Académicos

- Demostrar la implementación del **patrón repositorio.**
- Evidenciar la separación clara entre capas de la aplicación.
- Mostrar el desacople de la capa de persistencia para intercambio de bases de datos.
- Implementar versionamiento de APIs siguiendo estándares de la industria.
- Implementar paginación en las respuestas de las peticiones con gran cantidad de resultados.
- Aplicar mejores prácticas de seguridad usando GUIDs en lugar de IDs secuenciales.

## Arquitectura

### Estructura de Capas

```
Controllers → Services → Repositories (via Interfaces) → DB Context
                  ↓
              IRepositories (Interfaces)
```

### Componentes

- **Controllers**: Capa de presentación y manejo de HTTP.
- **Services**: Lógica de negocio y reglas de dominio.
- **Interfaces**: Contratos para desacoplamiento.
- **Repositories**: Implementaciones de acceso a datos.
- **Models**: Modelos de dominio.
- **DBContext**: Contextos y configuraciones de base de datos.

## Modelo de Datos

### Entidades Principales

#### Ubicaciones

Ubicaciones geográficas para las plantas de generación

- `Id` (UUID): Identificador único.
- `CodigoMunicipio` (TEXT): Código DIVIPOLA del departamento.
- `CodigoDepartamento` (TEXT): Código DIVIPOLA del municipio.
- `IsoDepartamento` (TEXT): Código ISO 3166-2:CO del departamento.
- `NombreDepartamento` (TEXT): Nombre oficial del departamento según DIVIPOLA.
- `NombreMunicipio` (TEXT): Nombre oficial del municipio según DIVIPOLA.

#### Tipos

Catálogo de tipos de fuentes energéticas.

- `Id` (UUID): Identificador único.
- `Nombre` (TEXT): Nombre del tipo de fuente.
- `Descripcion` (TEXT): Descripción detallada.
- `EsRenovable` (BOOLEAN): Indica si es fuente renovable.

#### Plantas

Registro de plantas generadoras de energía

- `Id` (UUID): Identificador único.
- `Nombre` (TEXT): Nombre de la planta.
- `TipoFuenteId` (UUID): Referencia al tipo de fuente.
- `UbicacionId` (UUID): Referencia a la ubicación geográfica.
- `Capacidad` (DOUBLE): Capacidad en Megavatios.

#### Produccion

Registro diario de producción energética.

- `Id` (UUID): Identificador único.
- `PlantaId` (UUID): Referencia a la planta.
- `Fecha` (DATE): Fecha del registro.
- `Valor` (DOUBLE): Producción en Megavatios.

### Tipos de Fuentes Energéticas

- Hidroeléctrica
- Solar Fotovoltáica
- Eólica
- Termoeléctrica por Combustibles Fósiles
- Geotérmica
- Nuclear

## Stack Tecnológico

- **Framework**: .NET 9.x
- **Base de Datos**: PostgreSQL 17.x / MongoDB 8.x
- **ORM**: Dapper (micro-ORM) 2.1.x
- **Documentación**: Swagger/OpenAPI
- **Identificadores**: UUID/GUID para seguridad en versión relacional
- **Driver DB Relacional**: Npgsql 9.0.3
- **Driver DB NoSQL**: MongoDB Driver 3.5.x


## Endpoints API

El versionamiento de los endpoints utilizará parámetro en el encabezado, en lugar de incluirlo en el URL

### Ubicaciones

```http
GET    /api/ubicaciones                      # Listar todas
GET    /api/ubicaciones/{id}                 # Obtener por ID
GET    /api/ubicaciones/{id}/plantas         # Obtener plantas asociadas por ID de la ubicación
```

### Tipos de Fuente

```http
GET    /api/tipos                            # Listar todos
GET    /api/tipos/{id}                       # Obtener por ID
GET    /api/tipos/{id}/plantas               # Obtener plantas asociadas por ID del tipo de fuente
POST   /api/tipos                            # Crear nuevo
PUT    /api/tipos                            # Actualizar completo
DELETE /api/tipos/{id}                       # Eliminar
```

### Plantas de Generación

```http
GET    /api/plantas                          # Listar todas
GET    /api/plantas/{id}                     # Obtener por ID
POST   /api/plantas                          # Crear nueva
PUT    /api/plantas/                         # Actualizar completa
DELETE /api/plantas/{id}                     # Eliminar
```

### Eventos de Producción Diaria

```http
GET    /api/produccion                       # Listar todos
GET    /api/produccion/{id}                  # Obtener por ID del evento
GET    /api/produccion/planta/{id}           # Por planta
GET    /api/produccion/fecha/{fecha}         # Por fecha
POST   /api/produccion                       # Crear registro
PUT    /api/produccion/                      # Actualizar completo
DELETE /api/produccion/{id}                  # Eliminar
```

### Estadísticas

```http
GET    /api/estadisticas/{fecha}                                  # Resumen diario
GET    /api/estadisticas/{fecha}/tipo/                            # Por tipo de fuente
GET    /api/estadisticas/{fecha}/ubicaciones                      # Por ubicación geográfica
GET    /api/estadisticas/rango-fechas?inicio={fecha}&fin={fecha}  # Por rango de fechas
```
