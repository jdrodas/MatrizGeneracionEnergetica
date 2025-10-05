using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using System.Globalization;

namespace mge.API.Services
{
    public class ProduccionService(IProduccionRepository produccionRepository,
                                    IPlantaRepository plantaRepository)
    {
        private readonly IProduccionRepository _produccionRepository = produccionRepository;
        private readonly IPlantaRepository _plantaRepository = plantaRepository;

        public async Task<ProduccionRespuesta> GetAllAsync(ProduccionParametrosConsulta produccionParametrosConsulta)
        {
            var laProduccion = await _produccionRepository
                .GetAllAsync();

            var respuestaProduccion = BuildProductionResponse(laProduccion, produccionParametrosConsulta);

            return respuestaProduccion;
        }

        public async Task<Produccion> GetByIdAsync(string eventoId)
        {
            Produccion unEvento = await _produccionRepository
                .GetByIdAsync(eventoId);

            if (string.IsNullOrEmpty(unEvento.Id))
                throw new AppValidationException($"Evento de producción no encontrado con el Id {eventoId}");

            return unEvento;
        }

        public async Task<List<Produccion>> GetAllByPlantIdAsync(string plantaId)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(plantaId);

            if (string.IsNullOrEmpty(unaPlanta.Id))
                throw new AppValidationException($"No hay planta registrada con el Id {plantaId}");


            var losEventos = await _produccionRepository
                .GetAllByPlantIdAsync(plantaId);

            if (losEventos.Count == 0)
                throw new AppValidationException($"No hay producción asociada a la planta {unaPlanta.Nombre}");

            return losEventos;
        }

        public async Task<List<Produccion>> GetAllByDateIdAsync(string fechaId)
        {
            bool fechaValida = DateTime
                .TryParseExact(fechaId, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaResultante);

            if (!fechaValida)
                throw new AppValidationException($"La fecha suministrada {fechaId} no tiene el formato DD-MM-YYYY");

            var losEventos = await _produccionRepository
                .GetAllByDateIdAsync(fechaResultante.ToString("yyyy-MM-dd"));

            if (losEventos.Count == 0)
                throw new AppValidationException($"No hay producción asociada a la fecha {fechaResultante}");

            return losEventos;
        }

        public async Task<Produccion> CreateAsync(Produccion unEvento)
        {
            unEvento.PlantaNombre = unEvento.PlantaNombre!.Trim();

            string resultadoValidacion = EvaluateEventDetailsAsync(unEvento);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var plantaExistente = await _plantaRepository
                .GetByDetailsAsync(unEvento.PlantaNombre, unEvento.PlantaId!);

            if (string.IsNullOrEmpty(plantaExistente.Id))
                throw new EmptyCollectionException($"No hay planta registrada con esa identificación");

            unEvento.PlantaId = plantaExistente.Id;
            unEvento.PlantaNombre = plantaExistente.Nombre;

            var eventoExistente = await _produccionRepository
                .GetByDetailsAsync(unEvento);

            if (eventoExistente.PlantaId == unEvento.PlantaId &&
                eventoExistente.Fecha == unEvento.Fecha &&
                eventoExistente.Valor == unEvento.Valor)
                return eventoExistente;

            if (unEvento.Valor > plantaExistente.Capacidad)
                throw new AppValidationException($"El evento supera la capacidad de producción " +
                    $"de la planta {plantaExistente.Nombre} de {plantaExistente.Capacidad} MW");

            try
            {
                bool resultadoAccion = await _produccionRepository
                    .CreateAsync(unEvento);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                eventoExistente = await _produccionRepository
                .GetByDetailsAsync(unEvento);

            }
            catch (DbOperationException)
            {
                throw;
            }

            return eventoExistente;
        }

        public async Task<Produccion> UpdateAsync(Produccion unEvento)
        {
            unEvento.PlantaNombre = unEvento.PlantaNombre!.Trim();

            string resultadoValidacion = EvaluateEventDetailsAsync(unEvento);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos si la planta existe
            var plantaExistente = await _plantaRepository
                .GetByDetailsAsync(unEvento.PlantaNombre, unEvento.PlantaId!);

            if (string.IsNullOrEmpty(plantaExistente.Id))
                throw new EmptyCollectionException($"No hay planta registrada con esa identificación");

            unEvento.PlantaId = plantaExistente.Id;
            unEvento.PlantaNombre = plantaExistente.Nombre;

            var eventoExistente = await _produccionRepository
                .GetByIdAsync(unEvento.Id!);

            if (string.IsNullOrEmpty(eventoExistente.Id))
                throw new EmptyCollectionException($"No existe un evento de producción registrado con ese Id");

            if (eventoExistente.PlantaId == unEvento.PlantaId &&
                eventoExistente.Fecha == unEvento.Fecha &&
                eventoExistente.Valor == unEvento.Valor)
                return eventoExistente;

            if (unEvento.Valor > plantaExistente.Capacidad)
                throw new AppValidationException($"El evento supera la capacidad de producción " +
                    $"de la planta {plantaExistente.Nombre} de {plantaExistente.Capacidad} MW");

            try
            {
                bool resultadoAccion = await _produccionRepository
                    .UpdateAsync(unEvento);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                eventoExistente = await _produccionRepository
                    .GetByIdAsync(unEvento.Id!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return eventoExistente;
        }

        public async Task<Produccion> RemoveAsync(string eventoId)
        {
            Produccion unEvento = await _produccionRepository
                .GetByIdAsync(eventoId);

            if (string.IsNullOrEmpty(unEvento.Id))
                throw new EmptyCollectionException($"No se encontró evento con el Id {eventoId}");

            try
            {
                bool resultadoAccion = await _produccionRepository
                    .RemoveAsync(eventoId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return unEvento;
        }

        private static string EvaluateEventDetailsAsync(Produccion unEvento)
        {

            //Se valida si viene con nombre y el guid es vacío
            if (string.IsNullOrEmpty(unEvento.PlantaNombre) && string.IsNullOrEmpty(unEvento.PlantaId))
                return "no se puede insertar un evento sin información de la planta";

            if (unEvento.Valor <= 0)
                return "El valor de la producción registrada en MW debe ser mayor que 0";

            bool fechaValida = DateTime
                            .TryParseExact(
                                unEvento.Fecha, "dd-MM-yyyy",
                                CultureInfo.InvariantCulture, DateTimeStyles.None,
                                out DateTime fechaResultante);

            if (!fechaValida)
                throw new AppValidationException($"La fecha suministrada {unEvento.Fecha} no tiene el formato DD-MM-YYYY");

            if (fechaResultante >= DateTime.Now)
                throw new AppValidationException($"No se puede registrar eventos de producción futuros. " +
                    $"La fecha actual es {DateTime.Now:dd-MM-yyyy}");

            unEvento.Fecha = fechaResultante.ToString("yyyy-MM-dd");

            return string.Empty;
        }

        private static ProduccionRespuesta BuildProductionResponse(IEnumerable<Produccion> laProduccion, ProduccionParametrosConsulta produccionParametrosConsulta)
        {
            // Calculamos items totales y cantidad de páginas
            var totalElementos = laProduccion.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalElementos / produccionParametrosConsulta.ElementosPorPagina);

            //Validamos que la página solicitada está dentro del rango permitido
            if (produccionParametrosConsulta.Pagina > totalPaginas && totalPaginas > 0)
                throw new AppValidationException($"La página solicitada No. {produccionParametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {produccionParametrosConsulta.ElementosPorPagina}");

            //Aplicamos la paginación
            laProduccion = laProduccion
                .Skip((produccionParametrosConsulta.Pagina - 1) * produccionParametrosConsulta.ElementosPorPagina)
                .Take(produccionParametrosConsulta.ElementosPorPagina);

            var respuestaProduccion = new ProduccionRespuesta
            {
                Tipo = "Producción",
                TotalElementos = totalElementos,
                Pagina = produccionParametrosConsulta.Pagina, // page
                ElementosPorPagina = produccionParametrosConsulta.ElementosPorPagina, // pageSize
                TotalPaginas = totalPaginas,
                Criterio = produccionParametrosConsulta.Criterio,
                Data = [.. laProduccion]
            };

            return respuestaProduccion;
        }
    }
}
