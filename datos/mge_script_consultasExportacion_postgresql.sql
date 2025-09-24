-- Scripts de clase - Agosto 28 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co


-- Proyecto: Matriz Generación Energetica - Seguimiento a la generación energética por tipo de fuente.
-- Motor de Base de datos: PostgreSQL 17.x

-- **********************************************
-- Consultas para exportar datos a mongoDB
-- **********************************************

-- Con el usuario mge_app

-- Ubicaciones
select distinct
    object_id _id,
    codigo_departamento,
    codigo_municipio,
    nombre_departamento,
    nombre_municipio,
    iso_departamento
from core.ubicaciones
order by nombre_departamento, nombre_municipio;

-- Tipos
select distinct
    object_id _id,
    nombre,
    descripcion,
    esrenovable
from core.tipos
order by nombre;

-- Plantas
select distinct
    p.object_id _id,
    p.nombre,
    p.capacidad,
    p.tipo_object_id tipo_id,
    t.nombre tipo_nombre,
    p.ubicacion_object_id ubicacion_id,
    (u.nombre_municipio || ', ' || u.nombre_departamento) ubicacion_nombre
from core.plantas p
    join core.tipos t on p.tipo_id = t.id
    join core.ubicaciones u on p.ubicacion_id = u.id
order by nombre;

-- Producción
select
    distinct p.object_id _id,
             planta_object_id planta_id,
             pt.nombre planta_nombre,
             p.valor,
             p.fecha
from core.produccion p join plantas pt on p.planta_id = pt.id;