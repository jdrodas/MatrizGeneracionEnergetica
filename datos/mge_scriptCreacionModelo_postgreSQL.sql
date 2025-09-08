-- Scripts de clase - Agosto 27 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Matriz Generación Energetica - Seguimiento a la generación energética por tipo de fuente.
-- Motor de Base de datos: PostgreSQL 17.x

-- ***********************************
-- Creación del modelo de datos
-- *********************************** 

-- Con el usuario mge_app

-- ****************************************
-- Creación de Tablas
-- ****************************************

create table core.ubicaciones
(
    id                      uuid default gen_random_uuid() constraint ubicaciones_pk primary key,    
    codigo_departamento     text not null,
    codigo_municipio        text not null,
    nombre_departamento     text not null,
    nombre_municipio        text not null,
    departamento            text default 'CO-000' not null
);

alter table ubicaciones add constraint ubicaciones_codigo_municipio_uk unique (codigo_municipio);
create unique index ubicacion_dpto_mpio_uk on core.ubicaciones (codigo_departamento, lower(nombre_municipio));

comment on table core.ubicaciones is 'Ubicaciones geográficas para las plantas de generación';
comment on column core.ubicaciones.id is 'ID de la ubicación';
comment on column core.ubicaciones.codigo_departamento is 'Código DIVIPOLA del departamento';
comment on column core.ubicaciones.codigo_municipio is 'Código DIVIPOLA del municipio';
comment on column core.ubicaciones.nombre_departamento is 'Nombre oficial del departamento según DIVIPOLA';
comment on column core.ubicaciones.nombre_municipio  is 'Nombre oficial del municipio según DIVIPOLA';
comment on column core.ubicaciones.iso_departamento is 'Código ISO 3166-2:CO del departamento';


create table core.tipos
(
    id          uuid default gen_random_uuid() constraint tipos_pk primary key,
    nombre      text    not null,
    descripcion text    not null,
    esrenovable boolean not null
);

comment on table core.tipos is 'Catálogo de tipos de fuentes energéticas';
comment on column core.tipos.id is 'ID del tipo de fuente energética';
comment on column core.tipos.nombre is 'Nombre el tipo e fuente';
comment on column core.tipos.descripcion is 'descripcion detallada del tipo de fuente';
comment on column core.tipos.esrenovable is 'Indica si es fuente renovable';


create table core.plantas
(
    id              uuid default gen_random_uuid() not null constraint plantas_pk primary key,
    nombre          text not null,
    tipo_id         uuid not null constraint plantas_tipo_fk references tipos,
    ubicacion_id    uuid not null constraint plantas_ubicacion_fk references ubicaciones,
    capacidad       double precision not null
);

create unique index planta_tipo_uk on core.plantas (lower(nombre), tipo_id);

comment on table core.plantas is 'Registro de plantas generadoras de energía';
comment on column core.plantas.id is 'ID de la planta generadora';
comment on column core.plantas.nombre is 'Nombre de la planta';
comment on column core.plantas.tipo_id is 'Tipo de fuente de energía utilizada por la planta';
comment on column core.plantas.ubicacion_id is 'ID de la ubicación geográfica de la planta';
comment on column core.plantas.capacidad is 'Capacidad instalada en MW de generación en Megavatios';


create table core.produccion
(
    id          uuid default gen_random_uuid() constraint produccion_pk primary key,
    planta_id   uuid not null constraint produccion_planta_fk references plantas,
    fecha       date not null,
    valor       double precision not null
);

create unique index planta_produccion_dia on core.produccion (planta_id,fecha);

comment on table core.produccion is 'Registro de la generación diaria de cada planta';
comment on column core.produccion.id is 'ID del evento de generación';
comment on column core.produccion.fecha is 'Fecha en la que se produce el registro de generación';
comment on column core.produccion.planta_id is 'ID de la planta que está produciendo energía';
comment on column core.produccion.valor  is 'Energía generada en MW por la planta en este día';

-- ****************************************
-- Creación de Vistas
-- ****************************************

create view core.v_info_plantas as(
select distinct
    p.id planta_id,
    p.nombre planta_nombre,
    p.capacidad,
    p.tipo_id,
    t.nombre tipo_nombre,
    t.esrenovable,
    p.ubicacion_id,
    u.iso_departamento,
    (u.nombre_municipio || ', '||  u.nombre_departamento) ubicacion_nombre    
from core.plantas p
    join core.tipos t on p.tipo_id = t.id
    join core.ubicaciones u on p.ubicacion_id = u.id
);

create view core.v_info_produccion_planta as (
select distinct
    pdn.id,
    pdn.planta_id,
    p.nombre planta_nombre,
    pdn.valor,
    pdn.fecha
from core.plantas p
    join core.produccion pdn on p.id = pdn.planta_id
);

create view core.v_produccion_tipo_dia as
(
select distinct pl.tipo_id,
                t.nombre,
                t.esrenovable,
                pdn.fecha,
                sum(pdn.valor)
from core.produccion pdn
         inner join plantas pl on pdn.planta_id = pl.id
         inner join tipos t on pl.tipo_id = t.id
group by pl.tipo_id,
         t.nombre,
         t.esrenovable,
         pdn.fecha
    );