using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Stocks.DTOs;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Repositories;

namespace ProyectoAW.Modules.Stocks.Services
{
    public class MovimientoStockService : IMovimientoStockService
    {
        private readonly IMovimientoStockRepository _repository;

        public MovimientoStockService(IMovimientoStockRepository repository) => _repository = repository;

        public Task<IReadOnlyList<Guia>> ListarGuiasParaLecturaAsync(int? idEstadoGuia)
        {
            if (idEstadoGuia.HasValue && !new[] { EstadoGuiasIds.Pendiente, EstadoGuiasIds.EnProceso }.Contains(idEstadoGuia.Value))
                throw new NegocioException("Solo se pueden listar guías pendientes o en proceso para lectura.");
            return _repository.ListarGuiasParaLecturaAsync(idEstadoGuia);
        }

        public async Task<Guia> ObtenerGuiaParaLecturaAsync(int idGuia)
        {
            var guia = await _repository.ObtenerGuiaParaLecturaAsync(idGuia);
            if (guia == null)
                throw new NegocioException($"No se encontró la guía {idGuia}.");
            ValidarGuiaAbierta(guia);
            return guia;
        }

        public async Task RegistrarLecturaAsync(int idGuia, RegistrarLecturaGuiaDto request)
        {
            var guia = await _repository.ObtenerGuiaParaLecturaAsync(idGuia);
            if (guia == null)
                throw new NegocioException($"No se encontró la guía {idGuia}.");
            ValidarGuiaAbierta(guia);

            if (!guia.Detalles.Any(d => d.IdProducto == request.IdProducto))
                throw new NegocioException("El producto leído no pertenece al detalle de la guía.");
            if (!await _repository.ExisteOperadorAsync(request.IdUsuarioOperador))
                throw new NegocioException("No se encontró el usuario operador.");

            await ValidarUbicacion(request.IdUbicacionOrigen, guia.IdAlmacenOrigen, "origen");
            await ValidarUbicacion(request.IdUbicacionDestino, guia.IdAlmacenDestino, "destino");
            await _repository.RegistrarLecturaAsync(new LecturaGuia
            {
                IdGuia = idGuia,
                IdProducto = request.IdProducto,
                IdUsuarioOperador = request.IdUsuarioOperador,
                Cantidad = request.Cantidad,
                CodigoLeido = request.CodigoLeido.Trim(),
                Lote = request.Lote?.Trim(),
                Serie = request.Serie?.Trim(),
                Observacion = request.Observacion?.Trim(),
                IdUbicacionOrigen = request.IdUbicacionOrigen,
                IdUbicacionDestino = request.IdUbicacionDestino,
                FechaRegistroUtc = DateTime.UtcNow
            }, guia);
        }

        private void ValidarGuiaAbierta(Guia guia)
        {
            if (guia.IdEstadoGuia != EstadoGuiasIds.Pendiente && guia.IdEstadoGuia != EstadoGuiasIds.EnProceso)
                throw new NegocioException("La guía no está disponible para lectura.");
        }

        private async Task ValidarUbicacion(int? idUbicacion, int? idAlmacen, string extremo)
        {
            if (!idUbicacion.HasValue) return;
            if (!idAlmacen.HasValue || !await _repository.UbicacionPerteneceAlmacenAsync(idUbicacion.Value, idAlmacen.Value))
                throw new NegocioException($"La ubicación de {extremo} no pertenece al almacén indicado en la guía.");
        }
    }
}


