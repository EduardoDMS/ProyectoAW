using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Repositories;

namespace ProyectoAW.Modules.Stocks.Services
{
    public class EntradaStockProcessor : IGuiaStockProcessor
    {
        private readonly IStockBalanceRepository _stockRepository;

        public EntradaStockProcessor(IStockBalanceRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public int IdTipoOperacionGuia => TipoOperacionGuiaIds.Entrada;
        public bool RequiereUbicacionOrigen => false;
        public bool RequiereUbicacionDestino => true;

        public void ValidarGuia(Guia guia) { }

        public int ObtenerAlmacenMovimiento(Guia guia) => guia.IdAlmacenDestino ?? 0;

        public async Task AplicarProductoAsync(
            Guia guia,
            DetalleGuia detalle,
            IReadOnlyList<LecturaGuia> lecturas)
        {
            var idAlmacen = guia.IdAlmacenDestino
                ?? throw new NegocioException("La guía de entrada no tiene almacén de destino.");
            var cantidadTotal = lecturas.Sum(lectura => (long)lectura.Cantidad);

            foreach (var grupo in lecturas.GroupBy(lectura => lectura.IdUbicacionDestino!.Value))
            {
                var cantidad = checked((int)grupo.Sum(lectura => (long)lectura.Cantidad));
                var stock = (await _stockRepository.ObtenerStockUbicacionAsync(
                    grupo.Key, detalle.IdProducto)).FirstOrDefault();

                if (stock is null)
                {
                    await _stockRepository.AñadiendoStockUbicacionAsync(new StockUbicacion
                    {
                        IdUbicacion = grupo.Key,
                        IdProducto = detalle.IdProducto,
                        CantidadActual = cantidad
                    });
                    continue;
                }

                stock.CantidadActual = checked(stock.CantidadActual + cantidad);
                stock.FechaUltimoRegistro = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
                await _stockRepository.ActualizarStockUbicacionAsync(stock);
            }

            var stockAlmacen = (await _stockRepository.ObtenerStockAlmacenAsync(
                idAlmacen, detalle.IdProducto)).FirstOrDefault();

            if (stockAlmacen is null)
            {
                await _stockRepository.CrearSaldoAlmacenAsync(new StockAlmacen
                {
                    IdAlmacen = idAlmacen,
                    IdProducto = detalle.IdProducto,
                    CantidadActual = checked((int)cantidadTotal)
                });
                return;
            }

            stockAlmacen.CantidadActual = checked(stockAlmacen.CantidadActual + (int)cantidadTotal);
            stockAlmacen.FechaUltimoRegitro = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
            await _stockRepository.ActualizarSaldoAlmacenAsync(stockAlmacen);
        }
    }
}
