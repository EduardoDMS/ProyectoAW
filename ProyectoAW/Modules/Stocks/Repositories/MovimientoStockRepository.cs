using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public class MovimientoStockRepository : IMovimientoStockRepository
    {
        private readonly AppDbContext _context;

        public MovimientoStockRepository(AppDbContext context) => _context = context;

        public async Task<Guia?> ObtenerGuiaParaLecturaAsync(int idGuia) => await ConsultaGuias()
            .FirstOrDefaultAsync(g => g.IdGuia == idGuia);

        public async Task<IReadOnlyList<Guia>> ListarGuiasParaLecturaAsync(int? idEstadoGuia)
        {
            var query = ConsultaGuias().AsNoTracking()
                .Where(g => g.IdEstadoGuia == EstadoGuiasIds.Pendiente || g.IdEstadoGuia == EstadoGuiasIds.EnProceso);
            if (idEstadoGuia.HasValue)
                query = query.Where(g => g.IdEstadoGuia == idEstadoGuia.Value);
            return await query.OrderBy(g => g.FechaProgramada).ToListAsync();
        }

        public Task<bool> ExisteOperadorAsync(int idOperador) =>
            _context.Set<Usuario>().AnyAsync(u => u.IdUsuario == idOperador);

        public Task<int> ObtenerCantidadLeidaAsync(int idGuia, int idProducto) =>
            _context.LecturasGuia.Where(l => l.IdGuia == idGuia && l.IdProducto == idProducto).SumAsync(l => l.Cantidad);
        public Task<bool> UbicacionPerteneceAlmacenAsync(int idUbicacion, int idAlmacen) =>
            _context.Set<Ubicacion>().AnyAsync(u => u.IdUbicacion == idUbicacion && u.IdAlmacen == idAlmacen);

        public async Task RegistrarLecturaAsync(LecturaGuia lectura, Guia guia)
        {
            _context.LecturasGuia.Add(lectura);
            if (guia.IdEstadoGuia == EstadoGuiasIds.Pendiente)
                guia.IdEstadoGuia = EstadoGuiasIds.EnProceso;
            await _context.SaveChangesAsync();
        }

        private IQueryable<Guia> ConsultaGuias() => _context.Guias
            .Include(g => g.TipoOperacion)
            .Include(g => g.Estado)
            .Include(g => g.Motivo)
            .Include(g => g.Proveedor)
            .Include(g => g.Cliente)
            .Include(g => g.AlmacenOrigen)
            .Include(g => g.AlmacenDestino)
            .Include(g => g.Detalles).ThenInclude(d => d.Producto);

        public Task<List<MovimientoStock>> ListarMovimientosStockAsync()
        {
            throw new NotImplementedException();
        }

        public void AgregarMovimientoAsync(MovimientoStock movimiento)
        {
            throw new NotImplementedException();
        }

        public Task GuardarCambiosAsync()
        {
            throw new NotImplementedException();
        }
    }
}



