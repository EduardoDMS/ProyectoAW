using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public class StockRepository : IStockRepository
    {
        public Task<IReadOnlyList<StockUbicacion>> ListarAsync(int? idAlmacen = null, int? idProducto = null, string? codigoUbicacion = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<int, long>> ObtenerDisponibilidadAsync(int idAlmacen, IReadOnlyCollection<int> idsProductos, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
