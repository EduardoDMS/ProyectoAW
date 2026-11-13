using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Migrations;

namespace ProjectAW.Modules.Stocks.Services;

public interface IGuiaStockProcessor
{
    int IdTipoOperacionGuia { get; }
    bool RequiereUbicacionOrigen { get; }
    bool RequiereUbicacionDestino { get; }

    void ValidarGuia(Guia guia);

    int ObtenerAlmacenMovimiento(Guia guia);

    Task AplicarProductoAsync(
        Guia guia,
        DetalleGuia detalle,
        IReadOnlyList<LecturaGuia> lecturas,
        CancellationToken ct = default);
}
