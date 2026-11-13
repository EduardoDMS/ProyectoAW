using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public interface IStockBalanceRepository
    {
        Task<List<StockUbicacion>> ObtenerStockUbicacionAsync(int idUbicacion, int idProducto);
        Task<List<StockUbicacion>> ObtenerStockUbicacionAlmacenAsync(int idAlmacen, int isProducto);
        Task<List<StockAlmacen>> ObtenerStockAlmacenAsync(int idAlmacen, int idProducto);
        
        
        Task AñadiendoStockUbicacionAsync(StockUbicacion saldo);
        Task ActualizarStockUbicacionAsync(StockUbicacion saldo);
        Task CrearSaldoAlmacenAsync(StockAlmacen saldo);
        Task ActualizarSaldoAlmacenAsync(StockAlmacen saldo);
    }

}
