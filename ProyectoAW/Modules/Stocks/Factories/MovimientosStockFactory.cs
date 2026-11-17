using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Services;

namespace ProyectoAW.Modules.Stocks.Factories
{
    public class MovimientosStockFactory
    {

        public static MovimientoStock CrearMovimiento(
        Guia guia,
        DetalleGuia detalle,
        IReadOnlyList<LecturaGuia> lecturas,
        IGuiaStockProcessor processor,
        int idOperador,
        int totalLeido,
        int cantidadEsperada)
        {
            var observacion = lecturas
                .Select(lectura => lectura.Observacion)
                .FirstOrDefault(texto => !string.IsNullOrWhiteSpace(texto)) ?? string.Empty;

            return new MovimientoStock
            {
                IdProducto = detalle.IdProducto,
                IdAlmacen = processor.ObtenerAlmacenMovimiento(guia),
                IdTipoOperacion = processor.IdTipoOperacionGuia,
                IdGuia = guia.IdGuia,
                IdUsuarioOperador = idOperador,
                Cantidad = totalLeido,
                CantidadDiferenciada = checked(totalLeido - cantidadEsperada),
                Observacion = observacion,
                FchMovimiento = DateTime.UtcNow,
                IdUbicacionOrigen = ObtenerUbicacionComun(lecturas.Select(lectura => lectura.IdUbicacionOrigen)),
                IdUbicacionDestino = ObtenerUbicacionComun(lecturas.Select(lectura => lectura.IdUbicacionDestino))
            };
        }


        public static int? ObtenerUbicacionComun(IEnumerable<int?> ubicaciones)
        {
            var ubicacionesDistintas = ubicaciones.Distinct().ToList();

            if (ubicacionesDistintas.Count != 1)
                return null;

            return ubicacionesDistintas[0];
        }


    }
}
