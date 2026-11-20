using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Repositories;

namespace ProyectoAW.Modules.Stocks.Services;

public class SalidaStockProcessor : IGuiaStockProcessor
{
    private readonly IStockBalanceRepository _stockRepository;

    public SalidaStockProcessor(IStockBalanceRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public int IdTipoOperacionGuia => TipoOperacionGuiaIds.Salida;
    public bool RequiereUbicacionOrigen => true;
    public bool RequiereUbicacionDestino => false;

    public void ValidarGuia(Guia guia) { }

    public int ObtenerAlmacenMovimiento(Guia guia) => guia.IdAlmacenOrigen ?? 0;

    public async Task AplicarProductoAsync(
        Guia guia,
        DetalleGuia detalle,
        IReadOnlyList<LecturaGuia> lecturas)
    {
        var idAlmacen = guia.IdAlmacenOrigen
            ?? throw new NegocioException("La guía de salida no tiene almacén de origen.");
        var cantidadTotal = lecturas.Sum(lectura => (long)lectura.Cantidad);

        foreach (var grupo in lecturas.GroupBy(lectura => lectura.IdUbicacionOrigen!.Value))
        {
            var cantidad = checked((int)grupo.Sum(lectura => (long)lectura.Cantidad));
            var stock = (await _stockRepository.ObtenerStockUbicacionAsync(
                grupo.Key, detalle.IdProducto)).FirstOrDefault();

            if (stock is null || stock.CantidadActual < cantidad)
                throw new NegocioException($"No hay stock suficiente del producto {detalle.IdProducto} en la ubicación {grupo.Key}.");

            stock.CantidadActual -= cantidad;
            stock.FechaUltimoRegistro = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
            await _stockRepository.ActualizarStockUbicacionAsync(stock);
        }

        var stockAlmacen = (await _stockRepository.ObtenerStockAlmacenAsync(
            idAlmacen, detalle.IdProducto)).FirstOrDefault();

        if (stockAlmacen is null || stockAlmacen.CantidadActual < cantidadTotal)
            throw new NegocioException($"No hay stock suficiente del producto {detalle.IdProducto} en el almacén {idAlmacen}.");

        stockAlmacen.CantidadActual -= (int)cantidadTotal;
        stockAlmacen.FechaUltimoRegitro = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        await _stockRepository.ActualizarSaldoAlmacenAsync(stockAlmacen);
    }
}
