using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public interface IMovimientoStockRepository
    {
        Task<List<MovimientoStock>> ListarMovimientosStockAsync();
        Task GuardarCambiosAsync();

        void AgregarMovimiento(MovimientoStock movimiento);
    }
}
