using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public class MovimientoStockRepository : IMovimientoStockRepository
    {
        private readonly AppDbContext _context;

        public MovimientoStockRepository(AppDbContext context) => _context = context;

        public Task<List<MovimientoStock>> ListarMovimientosStockAsync() =>
            _context.MovimientoStock
                .AsNoTracking()
                .Include(m => m.Producto)
                .Include(m => m.Almacen)
                .Include(m => m.OperacionGuias)
                .Include(m => m.Guia)
                .Include(m => m.Usuario)
                .Include(m => m.UbicacionOrigen)
                .Include(m => m.UbicacionDestino)
                .ToListAsync();

        public void AgregarMovimiento(MovimientoStock movimiento) =>
            _context.MovimientoStock.Add(movimiento);

        public Task GuardarCambiosAsync() => _context.SaveChangesAsync();
    }
}
