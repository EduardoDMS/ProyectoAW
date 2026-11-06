using ProyectoAW.Modules.Maestros.Productos.Entities;

namespace ProyectoAW.Modules.Maestros.Productos.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<Producto?> ObtenerPorCodigoAsync(string codigo);
    Task CrearAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
}
