using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public interface IStockRepository
    {
        // no aplicaremos esto vamos a entednderlo y estructurarlo bien primero :v
        Task<IReadOnlyList<StockUbicacion>> ListarAsync(int? idAlmacen = null, int? idProducto = null, string? codigoUbicacion = null, CancellationToken ct = default);
        Task<IReadOnlyDictionary<int, long>> ObtenerDisponibilidadAsync(
            int idAlmacen, IReadOnlyCollection<int> idsProductos, CancellationToken ct = default);
    }
}
