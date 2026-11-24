using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Maestros.Productos.Entities;

namespace ProyectoAW.Modules.Maestros.Productos.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        return await _context.Productos
                             .AsNoTracking()
                             .Include(u => u.UnidadMedida)
                             .Include(c => c.Categoria)
                             .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p=>p.IdProducto==id);
    }

    public async Task<Producto?> ObtenerPorCodigoAsync(string codigo)
    {
        return await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p=>p.Codigo==codigo);
    }

    public async Task CrearAsync(Producto producto)
    {
        await _context.Productos.AddAsync(producto);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Producto producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ProductoTieneUnidadDeMedidaAsociado(int idProducto)
    {
        return await _context.Productos
                             .AnyAsync(p => p.IdUnidadMedida == idProducto);
    }

    public async Task<bool> TieneMovimientosAsociadosAsync(int idProducto)
    {
        return await _context.MovimientoStock.AnyAsync(m => m.IdProducto == idProducto);
    }

    public async Task<bool> TieneStockAsociadoAsync(int idProducto)
    {
        return await _context.StockUbicacion.AnyAsync(s => s.IdProducto == idProducto);
    }

    public async Task EliminarProductoAsync(Producto producto)
    {
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Producto>> ObtenerProductosXAlmacenAsync(int idAlmacenOrigen)
    {
        return await _context.Productos.AsNoTracking()
                                          .Include(p => p.Categoria)
                                          .Include(p => p.UnidadMedida)
                                          .Include(p => p.StockAlmacen!.Where(s => s.IdAlmacen == idAlmacenOrigen))
                                          .Where(p => p.Activo && p.StockAlmacen!.Any(s => s.IdAlmacen == idAlmacenOrigen && s.CantidadActual > 0))
                                          .OrderBy(p => p.Nombre)
                                          .ToListAsync();
    }
}
