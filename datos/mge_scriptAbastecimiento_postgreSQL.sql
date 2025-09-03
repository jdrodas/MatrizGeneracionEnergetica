-- Scripts de clase - Agosto 27 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Matriz Generación Energetica - Seguimiento a la generación energética por tipo de fuente.
-- Motor de Base de datos: PostgreSQL 17.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull postgres:latest

-- Crear el contenedor
docker run --name pgsql-mge -e POSTGRES_PASSWORD=unaClav3 -d -p 5432:5432 postgres:latest


-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Postgres:

-- crear el esquema la base de datos
create database mge_db;

-- Conectarse a la base de datos
\c mge_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;

-- crear el usuario con el que se implementará la creación del modelo
create user mge_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database mge_db to mge_app;
grant create on database mge_db to mge_app;
grant create, usage on schema core to mge_app;
alter user mge_app set search_path to core;

-- crear el usuario con el que se conectará la aplicación
create user mge_usr with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database mge_db to mge_usr;
grant usage on schema core to mge_usr;

-- Privilegios sobre tablas existentes
grant select, insert, update, delete, trigger on all tables in schema core to mge_usr;

-- privilegios sobre secuencias existentes
grant usage, select on all sequences in schema core to mge_usr;

-- privilegios sobre funciones existentes
grant execute on all functions in schema core to mge_usr;

-- privilegios sobre procedimientos existentes
grant execute on all procedures in schema core to mge_usr;

-- privilegios sobre objetos futuros
alter default privileges in schema core grant select, insert, update, delete on tables TO mge_usr;
alter default privileges in schema core grant execute on routines to mge_usr;

alter user mge_usr set search_path to core;

-- Activar la extensión que permite el uso de UUID
create extension if not exists "uuid-ossp";