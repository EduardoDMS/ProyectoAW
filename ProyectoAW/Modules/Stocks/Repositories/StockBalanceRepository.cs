using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Stocks.Entities;
using System.Collections;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public class StockBalanceRepository : IStockBalanceRepository
    {

        private readonly AppDbContext _context;

        public StockBalanceRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task ActualizarSaldoAlmacenAsync(StockAlmacen saldo)
        {
            _context.StockAlmacen.Update(saldo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarStockUbicacionAsync(StockUbicacion saldo)
        {
            _context.StockUbicacion.Update(saldo);
            await _context.SaveChangesAsync();
        }

        public async Task AñadiendoStockUbicacionAsync(StockUbicacion saldo)
        {
            _context.StockUbicacion.Update(saldo);
            await _context.SaveChangesAsync();
        }

        public async Task CrearSaldoAlmacenAsync(StockAlmacen saldo)
        {
            await _context.StockAlmacen.AddAsync(saldo);
            await _context.SaveChangesAsync();
        }

        public Task<List<StockAlmacen>> ObtenerStockAlmacenAsync(int idAlmacen, int idProducto)
        {
            return _context.StockAlmacen
            .Where(stock => stock.IdAlmacen == idAlmacen && stock.IdProducto == idProducto)
            .ToListAsync();
        }

        public Task<List<StockUbicacion>> ObtenerStockUbicacionAlmacenAsync(int idAlmacen, int isProducto)
        {
            return _context.StockUbicacion
                    .Include(stock => stock.IdUbicacion)
                    .Where(stock => stock.IdProducto == isProducto &&
                                    stock.Ubicacion!.IdAlmacen == idAlmacen)
                    .ToListAsync();
        }

        public Task<List<StockUbicacion>> ObtenerStockUbicacionAsync(int idUbicacion, int idProducto)
        {
            return _context.StockUbicacion.Where(saldo => saldo.IdUbicacion == idUbicacion && saldo.IdProducto == idProducto)
                                          .ToListAsync();
        }
    }
}
