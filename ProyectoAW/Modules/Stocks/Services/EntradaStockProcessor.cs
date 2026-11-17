using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Services
{
    public class EntradaStockProcessor : IGuiaStockProcessor
    {
        public int IdTipoOperacionGuia => 1;

        public bool RequiereUbicacionOrigen => false;

        public bool RequiereUbicacionDestino =>true;

        public void ValidarGuia(Guia guia) { }

        public int ObtenerAlmacenMovimiento(Guia guia)
                    => guia.IdAlmacenDestino ?? 0;

        public Task AplicarProductoAsync(Guia guia, DetalleGuia detalle, IReadOnlyList<LecturaGuia> lecturas)
        {
            return Task.CompletedTask;
        }
    }
}
