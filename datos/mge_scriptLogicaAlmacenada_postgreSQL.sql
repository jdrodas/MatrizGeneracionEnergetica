-- Scripts de clase - Agosto 28 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co


-- Proyecto: Matriz Generación Energetica - Seguimiento a la generación energética por tipo de fuente.
-- Motor de Base de datos: PostgreSQL 17.x

-- ***********************************
-- Creación de procedimientos
-- *********************************** 

-- Con el usuario mge_app

-- ### Tipos de Fuente ####

-- p_inserta_tipo
create procedure core.p_inserta_tipo(
                            in p_nombre text, 
                            in p_descripcion text, 
                            in p_esrenovable boolean)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_descripcion is null or
           length(p_nombre) = 0 or
           length(p_descripcion) = 0 then
               raise exception 'El nombre del tipo o su descripcion no pueden ser nulos';
        end if;

        -- Validación de cantidad de registros con ese nombre
        select count(id) into l_total_registros
        from core.tipos
        where lower(p_nombre) = lower(nombre);

        if l_total_registros != 0  then
            raise exception 'ya existe ese tipo de fuente';
        end if;

        insert into core.tipos(nombre,descripcion,esrenovable)
        values (initcap(p_nombre), lower(p_descripcion),p_esrenovable);
    end;
$$;


-- p_actualiza_tipo
create procedure core.p_actualiza_tipo(
                            in p_id uuid,
                            in p_nombre text,
                            in p_descripcion text,
                            in p_esrenovable boolean)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_descripcion is null or
           length(p_nombre) = 0 or
           length(p_descripcion) = 0 then
               raise exception 'El nombre del tipo o su descripcion no pueden ser nulos';
        end if;

        select count(id) into l_total_registros
        from core.tipos
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un tipo registrado con ese Id';
        end if;

        -- Validación de cantidad de tipos con ese nombre
        select count(id) into l_total_registros
        from core.tipos
        where lower(nombre) = lower(p_nombre)
        and lower(descripcion) = lower(p_descripcion)
        and id != p_id;

        if l_total_registros > 0  then
            raise exception 'Ya existe un tipo registrado con ese nombre y descripción';
        end if;

        update core.tipos
        set
            nombre = initcap(p_nombre),
            descripcion = lower(p_descripcion),
            esrenovable = p_esrenovable
        where id = p_id;
    end;
$$;

-- p_elimina_tipo
create procedure core.p_elimina_tipo(
                            in p_id uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin

        -- Cuantos tipos existen con el uuid proporcionado
        select count(id) into l_total_registros
        from core.tipos
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un tipo registrado con ese Guid';
        end if;

        -- Cuantos plantas están asociadas a este tipo
        select count(tipo_id) into l_total_registros
        from core.v_info_plantas
        where tipo_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay plantas que dependen de este tipo.';
        end if;

        delete from core.tipos
        where id = p_id;

    end;
$$;