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
                            in p_nombre         text, 
                            in p_descripcion    text, 
                            in p_esrenovable    boolean)
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
                            in p_id             uuid,
                            in p_nombre         text,
                            in p_descripcion    text,
                            in p_esrenovable    boolean)
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


-- ### Plantas de Generación ####

-- p_inserta_planta
create procedure core.p_inserta_planta(
                            in p_nombre         text, 
                            in p_tipo_id        uuid, 
                            in p_ubicacion_id   uuid, 
                            in p_capacidad      double precision)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or        
           length(p_nombre) = 0  then
               raise exception 'El nombre de la planta no puede ser nulo';
        end if;

        if p_capacidad <= 0  then
               raise exception 'La capacidad de la planta debe ser mayor que 0 MW';
        end if;        

        -- Validación de cantidad de registros con ese nombre y tipo de fuente
        select count(id) into l_total_registros
        from core.plantas
        where lower(p_nombre) = lower(nombre)
        and p_tipo_id = tipo_id;

        if l_total_registros != 0  then
            raise exception 'ya existe una planta con ese nombre y con ese tipo de fuente';
        end if;

        -- Validamos que el tipo de fuente sea válido
        select count(id) into l_total_registros
        from core.tipos
        where p_tipo_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe un tipo de fuente con ese ID';
        end if;        

        -- Validamos que la ubicación se válida
        select count(id) into l_total_registros
        from core.ubicaciones
        where p_ubicacion_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe una ubicación con ese ID';
        end if;        


        insert into core.plantas(nombre, capacidad, tipo_id, ubicacion_id)
        values (initcap(p_nombre), p_capacidad, p_tipo_id, p_ubicacion_id);
    end;
$$;


-- p_actualiza_planta
create procedure core.p_actualiza_planta(
                            in p_id             uuid,    
                            in p_nombre         text, 
                            in p_tipo_id        uuid, 
                            in p_ubicacion_id   uuid, 
                            in p_capacidad      decimal)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.plantas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una planta registrada con ese Id';
        end if;

        -- Validamos que el tipo de fuente sea válido
        select count(id) into l_total_registros
        from core.tipos
        where p_tipo_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe un tipo de fuente con ese ID';
        end if;        

        -- Validamos que la ubicación se válida
        select count(id) into l_total_registros
        from core.ubicaciones
        where p_ubicacion_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe una ubicación con ese ID';
        end if;          

        if p_nombre is null or        
           length(p_nombre) = 0  then
               raise exception 'El nombre de la planta no puede ser nulo';
        end if;

        if p_capacidad <= 0  then
               raise exception 'La capacidad de la planta debe ser mayor que 0 MW';
        end if;        

        -- Validación de cantidad de registros con ese nombre y tipo de fuente,
        -- diferentes al que estamos actualizando
        select count(id) into l_total_registros
        from core.plantas
        where lower(p_nombre) = lower(nombre)
        and p_tipo_id = tipo_id
        and p_id != id;

        if l_total_registros != 0  then
            raise exception 'ya existe ese otra planta con ese nombre con ese tipo de fuente';
        end if;

        -- Validamos que el tipo de fuente sea válido
        select count(id) into l_total_registros
        from core.tipos
        where p_tipo_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe un tipo de fuente con ese ID';
        end if;   

        update core.plantas
        set
            nombre = initcap(p_nombre),
            capacidad = p_capacidad,
            ubicacion_id = p_ubicacion_id,
            tipo_id = p_tipo_id
        where p_id = id;     
    end;
$$;

-- p_elimina_planta
create procedure core.p_elimina_planta(
                            in p_id uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin

        -- Cuantos plantas existen con el uuid proporcionado
        select count(id) into l_total_registros
        from core.plantas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una planta registrada con ese Guid';
        end if;

        -- Hay producción para esta planta
        select count(planta_id) into l_total_registros
        from core.v_info_produccion_planta
        where planta_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay producción registrada para esta planta.';
        end if;

        delete from core.plantas
        where id = p_id;

    end;
$$;


-- ### Producción de Energía por día y planta ####

-- p_inserta_produccion
create procedure core.p_inserta_produccion(
                            in p_planta_id      uuid, 
                            in p_fecha          date, 
                            in p_produccion     decimal)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        -- Validamos que la planta sea válida
        select count(id) into l_total_registros
        from core.plantas
        where p_planta_id = id;   

        if l_total_registros = 0  then
            raise exception 'no existe una planta con ese ID';
        end if;           

        if p_produccion <= 0  then
               raise exception 'La producción de la planta debe ser mayor que 0 MW';
        end if;        

        -- Validación de cantidad de registros para esa planta y esa fecha
        select count(id) into l_total_registros
        from core.produccion
        where p_planta_id = planta_id
        and p_fecha = fecha;

        if l_total_registros != 0  then
            raise exception 'ya existe ese registro de producción para esa planta en esa fecha';
        end if;

        insert into core.produccion(planta_id, fecha, produccion)
        values (p_planta_id, p_fecha, p_produccion);
    end;
$$;


-- p_actualiza_produccion
create procedure core.p_actualiza_produccion(
                            in p_id             uuid,
                            in p_planta_id      uuid, 
                            in p_fecha          date, 
                            in p_produccion     decimal)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        -- Validamos que la planta sea válida
        select count(id) into l_total_registros
        from core.plantas
        where p_planta_id = id;   

        if l_total_registros = 0  then
            raise exception 'no existe una planta con ese ID';
        end if;           

        if p_produccion <= 0  then
               raise exception 'La producción de la planta debe ser mayor que 0 MW';
        end if;        

        -- Validación de cantidad de registros para esa planta y esa fecha diferentes al evento
        select count(id) into l_total_registros
        from core.produccion
        where p_planta_id = planta_id
        and p_fecha = fecha
        and p_id != id;

        if l_total_registros != 0  then
            raise exception 'ya existe otro registro de producción para esa planta en esa fecha';
        end if;

        update core.produccion 
        set 
            planta_id = p_planta_id,
            fecha = p_fecha,
            produccion = p_produccion
        where id = p_id;

        insert into core.produccion(planta_id, fecha, produccion)
        values (p_planta_id, p_fecha, p_produccion);
    end;
$$;


-- p_elimina_produccion
create procedure core.p_elimina_produccion(
                            in p_id uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin

        -- Cuantos eventos de producción existen con el uuid proporcionado
        select count(id) into l_total_registros
        from core.produccion
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe registro de producción para una planta registrada con ese Guid';
        end if;

        delete from core.produccion
        where id = p_id;

    end;
$$;