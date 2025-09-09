using mge.API.Exceptions;
using mge.API.Interfaces;
using mge.API.Models;
using System.Globalization;

namespace mge.API.Services
{
    public class ProducccionService(IProduccionRepository produccionRepository,
                                    IPlantaRepository plantaRepository)
    {
        private readonly IProduccionRepository _produccionRepository = produccionRepository;
        private readonly IPlantaRepository _plantaRepository = plantaRepository;

        public async Task<List<Produccion>> GetAllAsync()
        {
            return await _produccionRepository
                .GetAllAsync();
        }

        public async Task<Produccion> GetByIdAsync(Guid evento_id)
        {
            Produccion unEvento = await _produccionRepository
                .GetByIdAsync(evento_id);

            if (unEvento.Id == Guid.Empty)
                throw new AppValidationException($"Evento de producción no encontrado con el Id {evento_id}");

            return unEvento;
        }

        public async Task<List<Produccion>> GetAllByPlantIdAsync(Guid planta_id)
        {
            Planta unaPlanta = await _plantaRepository
                .GetByIdAsync(planta_id);

            if (unaPlanta.Id == Guid.Empty)
                throw new AppValidationException($"No hay planta registrada con el Id {planta_id}");


            var LosEventos = await _produccionRepository
                .GetAllByPlantIdAsync(planta_id);

            if (LosEventos.Count == 0)
                throw new AppValidationException($"No hay producción asociada a la planta {unaPlanta.Nombre}");

            return LosEventos;
        }

        public async Task<List<Produccion>> GetAllByDateIdAsync(string fecha_id)
        {

            bool fechaValida = DateTime
                .TryParseExact(fecha_id, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);

            if (!fechaValida)
                throw new AppValidationException($"La fecha suministrada {fecha_id} no tiene el formato DD-MM-YYYY");



            var LosEventos = await _produccionRepository
                .GetAllByDateIdAsync(fecha_id);

            if (LosEventos.Count == 0)
                throw new AppValidationException($"No hay producción asociada a la fecha {fecha_id}");

            return LosEventos;
        }
    }
}
