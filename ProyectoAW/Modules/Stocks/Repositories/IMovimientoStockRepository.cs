using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public interface IMovimientoStockRepository
    {
        Task<IReadOnlyList<MovimientoStock>> ListarMovimientosStockAsync(int? idGuia, int? idAlmacen, int? idOperacion);
        Task GuardarCambiosAsync();
        Task<IReadOnlyList<LecturaGuia>> ObtenerLecturasParaFinalizarAsync(int idGuia);
        void AgregarMovimiento(MovimientoStock movimiento);
        Task<bool> TieneUbicacionesDespachoActivasAsync(int idAlmacen);
    }
}
