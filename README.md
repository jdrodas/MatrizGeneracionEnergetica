# Matriz Generación Energetica
Repositorio del proyecto de seguimiento a la generación energética por tipo de fuente.

API REST desarrollada como ejercicio demostrativo para el curso de **Tópicos Avanzados de Bases de Datos**, enfocada en la implementación del **patrón repositorio** y la separación por capas. El dominio de problema aborda el registro de composición de la matriz energética de un país, permitiendo el seguimiento de la producción diaria de diferentes fuentes de energía.

## Objetivos Académicos

- Demostrar la implementación del **patrón repositorio**
- Evidenciar la separación clara entre capas de la aplicación
- Mostrar el desacople de la capa de persistencia para intercambio de bases de datos
- Implementar versionamiento de APIs siguiendo estándares de la industria
- Aplicar mejores prácticas de seguridad usando GUIDs en lugar de IDs secuenciales

## Arquitectura

### Estructura de Capas
```
Controllers → Services → Repositories (via Interfaces) → DB Context
                  ↓
              IRepositories (Interfaces)
```

### Componentes
- **Controllers**: Capa de presentación y manejo de HTTP
- **Services**: Lógica de negocio y reglas de dominio
- **Interfaces**: Contratos para desacoplamiento
- **Repositories**: Implementaciones de acceso a datos
- **Entities**: Modelos de dominio
- **Data**: Contextos y configuraciones de base de datos

## Modelo de Datos

### Entidades Principales

#### Tipos
Catálogo de tipos de fuentes energéticas
- `Id` (UUID): Identificador único
- `Nombre` (VARCHAR): Nombre del tipo de fuente
- `Descripcion` (VARCHAR): Descripción detallada
- `EsRenovable` (BOOLEAN): Indica si es fuente renovable

#### Plantas
Registro de plantas generadoras de energía
- `Id` (UUID): Identificador único
- `Nombre` (VARCHAR): Nombre de la planta
- `TipoFuenteId` (UUID): Referencia al tipo de fuente
- `CapacidadInstalada` (DECIMAL): Capacidad en Megavatios

#### Produccion
Registro diario de producción energética
- `Id` (UUID): Identificador único
- `PlantaId` (UUID): Referencia a la planta
- `Fecha` (DATE): Fecha del registro
- `ProduccionMW` (DECIMAL): Producción en Megavatios

### Tipos de Fuentes Energéticas
- Hidroeléctrica
- Solar Fotovoltáica
- Eólica
- Termoeléctrica por Combustibles Fósiles
- Nuclear

## Stack Tecnológico

- **Framework**: .NET 9.x
- **Base de Datos**: PostgreSQL
- **ORM**: Dapper (micro-ORM)
- **Documentación**: Swagger/OpenAPI
- **Identificadores**: UUID/GUID para seguridad


## Endpoints API

### Versión 1.0

#### Tipos de Fuente
```http
GET    /api/v1/tipos-fuente                    # Listar todos
GET    /api/v1/tipos-fuente/{id}               # Obtener por ID
POST   /api/v1/tipos-fuente                    # Crear nuevo
PUT    /api/v1/tipos-fuente/{id}               # Actualizar completo
DELETE /api/v1/tipos-fuente/{id}               # Eliminar
```

#### Plantas de Generación
```http
GET    /api/v1/plantas                         # Listar todas
GET    /api/v1/plantas/{id}                    # Obtener por ID
POST   /api/v1/plantas                         # Crear nueva
PUT    /api/v1/plantas/{id}                    # Actualizar completa
DELETE /api/v1/plantas/{id}                    # Eliminar
```

#### Producción Diaria
```http
GET    /api/v1/produccion-diaria               # Listar registros
POST   /api/v1/produccion-diaria               # Crear registro
PUT    /api/v1/produccion-diaria/{id}          # Actualizar completo
DELETE /api/v1/produccion-diaria/{id}          # Eliminar
GET    /api/v1/produccion-diaria/planta/{id}   # Por planta
GET    /api/v1/produccion-diaria/fecha/{fecha} # Por fecha
```

#### Estadísticas
```http
GET    /api/v1/estadisticas/diarias/{fecha}                          # Resumen diario
GET    /api/v1/estadisticas/por-tipo/{fecha}                         # Por tipo de fuente
GET    /api/v1/estadisticas/rango-fechas?inicio={fecha}&fin={fecha}  # Por rango de fechas
