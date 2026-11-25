using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public class MovimientoStockRepository : IMovimientoStockRepository
    {
        private readonly AppDbContext _context;

        public MovimientoStockRepository(AppDbContext context) => _context = context;

        public async Task<IReadOnlyList<MovimientoStock>> ListarMovimientosStockAsync(int? idGuia, int? idAlmacen, int? idOperacion)
        {
          var query = _context.MovimientoStock
                .AsNoTracking()
                .Include(m => m.Producto)
                .Include(m => m.Almacen)
                .Include(m => m.OperacionGuias)
                .Include(m => m.Guia).ThenInclude(g => g!.Proveedor)
                .Include(m => m.Guia).ThenInclude(g => g!.Detalles)
                .Include(m => m.Guia).ThenInclude(g => g!.Motivo)
                .Include(m => m.Guia).ThenInclude(g => g!.Cliente)
                .Include(m => m.Guia).ThenInclude(g => g!.AlmacenOrigen)
                .Include(m => m.Guia).ThenInclude(g => g!.AlmacenDestino)
                .Include(m => m.Usuario)
                .Include(m => m.UbicacionOrigen)
                .Include(m => m.UbicacionDestino)
                .AsQueryable();

            if (idGuia.HasValue)
                query = query.Where(m => m.IdGuia == idGuia.Value);

            if (idAlmacen.HasValue)
            {
                query = query.Where(m =>
                        m.IdAlmacen == idAlmacen.Value ||
                        m.Guia!.IdAlmacenOrigen == idAlmacen.Value ||
                        m.Guia.IdAlmacenDestino == idAlmacen.Value);
            }

            if (idOperacion.HasValue)
            {
                query = query.Where(m => m.IdTipoOperacion == idOperacion.Value);
            }
            return await query
                    .OrderByDescending(m => m.FchMovimiento).ToListAsync();
        }

        public void AgregarMovimiento(MovimientoStock movimiento) =>
            _context.MovimientoStock.Add(movimiento);

        public Task GuardarCambiosAsync() => _context.SaveChangesAsync();

        public async Task<IReadOnlyList<LecturaGuia>> ObtenerLecturasParaFinalizarAsync(int idGuia)
            => await _context.LecturasGuia
                                 .AsNoTracking()
                                 .Where(lectura => lectura.IdGuia == idGuia)
                                 .OrderBy(lectura => lectura.IdLecturaGuia)
                                 .ToListAsync();

        public Task<bool> TieneUbicacionesDespachoActivasAsync(int idAlmacen)
        {
            return _context.Ubicaciones
                           .AsNoTracking()
                           .AnyAsync(ubicacion =>
                                     ubicacion.IdAlmacen == idAlmacen &&
                                     ubicacion.Activo &&
                                     _context.UbicacionesDespacho.Any(despacho =>
                                                                      despacho.IdAlmacen == idAlmacen &&
                                                                      despacho.IdUbicacion == ubicacion.IdUbicacion));

        }
    }
}
