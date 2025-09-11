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

        public async Task<List<Produccion>> GetAllAsync()
        {
            return await _produccionRepository
                .GetAllAsync();
        }

        public async Task<Produccion> GetByIdAsync(Guid eventoId)
        {
            Produccion unEvento = await _produccionRepository
                .GetByIdAsync(eventoId);

            if (unEvento.Id == Guid.Empty)
                throw new AppValidationException($"Evento de producción no encontrado con el Id {eventoId}");

            return unEvento;
        }

        public async Task<List<Produccion>> GetAllByPlantIdAsync(Guid plantaId)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(plantaId);

            if (unaPlanta.Id == Guid.Empty)
                throw new AppValidationException($"No hay planta registrada con el Id {plantaId}");


            var LosEventos = await _produccionRepository
                .GetAllByPlantIdAsync(plantaId);

            if (LosEventos.Count == 0)
                throw new AppValidationException($"No hay producción asociada a la planta {unaPlanta.Nombre}");

            return LosEventos;
        }

        public async Task<List<Produccion>> GetAllByDateIdAsync(string fechaId)
        {
            bool fechaValida = DateTime
                .TryParseExact(fechaId, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaResultante);

            if (!fechaValida)
                throw new AppValidationException($"La fecha suministrada {fechaId} no tiene el formato DD-MM-YYYY");

            var LosEventos = await _produccionRepository
                .GetAllByDateIdAsync(fechaId);

            if (LosEventos.Count == 0)
                throw new AppValidationException($"No hay producción asociada a la fecha {fechaResultante}");

            return LosEventos;
        }

        public async Task<Produccion> CreateAsync(Produccion unEvento)
        {
            unEvento.PlantaNombre = unEvento.PlantaNombre!.Trim();

            string resultadoValidacion = EvaluateEventDetailsAsync(unEvento);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            //Validamos si la planta existe
            var plantaExistente = await _plantaRepository
                .GetByDetailsAsync(unEvento.PlantaNombre, unEvento.PlantaId);

            if (plantaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No hay planta registrada con esa identificación");

            unEvento.PlantaId = plantaExistente.Id;

            //Validamos si ya hay un evento de producción de esa planta para la fecha
            var eventoExistente = await _produccionRepository
                .GetByDetailsAsync(unEvento);


            //Si existe y los datos son iguales, se retorna el objeto para garantizar idempotencia
            if (eventoExistente.PlantaId == unEvento.PlantaId &&
                eventoExistente.Fecha == unEvento.Fecha &&
                eventoExistente.Valor == unEvento.Valor)
                return eventoExistente;

            //Validamos que la producción no supere la capacidad de la planta
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
                .GetByDetailsAsync(unEvento.PlantaNombre, unEvento.PlantaId);

            if (plantaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No hay planta registrada con esa identificación");

            unEvento.PlantaId = plantaExistente.Id;

            //Validamos si ya hay un evento de producción con ese Id
            var eventoExistente = await _produccionRepository
                .GetByIdAsync(unEvento.Id);

            if (eventoExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe un evento de producción registrado con ese Id");

            //Si existe y los datos son iguales, se retorna el objeto para garantizar idempotencia
            if (eventoExistente.PlantaId == unEvento.PlantaId &&
                eventoExistente.Fecha == unEvento.Fecha &&
                eventoExistente.Valor == unEvento.Valor)
                return eventoExistente;

            //Validamos que la producción no supere la capacidad de la planta
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
                    .GetByIdAsync(unEvento.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return eventoExistente;
        }

        public async Task<Produccion> RemoveAsync(Guid eventoId)
        {
            Produccion unEvento = await _produccionRepository
                .GetByIdAsync(eventoId);

            if (unEvento.Id == Guid.Empty)
                throw new AppValidationException($"No se encontró evento con el Id {eventoId}");

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
            if (unEvento.PlantaNombre!.Length == 0 && unEvento.PlantaId == Guid.Empty)
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

            return string.Empty;
        }
    }
}
