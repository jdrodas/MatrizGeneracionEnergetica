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
    }
}
