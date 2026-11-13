using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infraestructure.RP;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.Productos.Entities;
using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
using ProyectoAW.Modules.Stocks.Entities;

namespace ProyectoAW.Modules.Stocks.Repositories
{
    public class LecturaGuiaRepository : ILecturaGuiaRepository
    {
        private readonly AppDbContext _context;

        public LecturaGuiaRepository(AppDbContext context) 
        { 
            _context = context; 
        }

        public Task<Guia?> ObtenerGuiaParaLecturaAsync(int idGuia)
        {
            return
            ConsultaGuias()
            .FirstOrDefaultAsync(g => g.IdGuia == idGuia);
        }
        
        public Task<Guia?> ObtenerGuiaOperadorAsync(int idGuia, int idOperador)
        {
            return 
            ConsultaGuias()
                .AsNoTracking()
                .FirstOrDefaultAsync(guia =>
                        guia.IdGuia == idGuia &&
                        (guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada || guia.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida) &&
                        guia.IdUsuarioModificacion == idOperador &&
                        (guia.IdEstadoGuia == EstadoGuiasIds.EnProceso || guia.IdEstadoGuia == EstadoGuiasIds.Atendida));
        }
        
        public async Task<IReadOnlyList<Guia>> ListarGuiasParaLecturaAsync(int? idEstadoGuia)
        {
            var query = ConsultaGuias().AsNoTracking()
                .Where(g => g.IdEstadoGuia == EstadoGuiasIds.Pendiente || g.IdEstadoGuia == EstadoGuiasIds.EnProceso);
            if (idEstadoGuia.HasValue)
                query = query.Where(g => g.IdEstadoGuia == idEstadoGuia.Value);
            return await query.OrderBy(g => g.FechaProgramada).ToListAsync();
        }

        public async Task<IReadOnlyList<Guia>> ListarGuiasParaOperadorAsync(int idOperador, int? idEstadoGuia)
        {
            var query = ConsultaGuias()
                        .AsNoTracking()
                        .Where(g =>
                                g.IdUsuarioModificacion == idOperador &&
                                (g.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada ||
                                 g.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida) &&
                                (g.IdEstadoGuia == EstadoGuiasIds.EnProceso ||
                                 g.IdEstadoGuia == EstadoGuiasIds.Atendida));

            if (idEstadoGuia.HasValue)
                query = query.Where(guia => guia.IdEstadoGuia == idEstadoGuia.Value);

            return await query
                .OrderByDescending(guia => guia.FechaActualizada ?? guia.FechaProgramada)
                .ToListAsync();

        }

        public async Task<IReadOnlyList<LecturaGuia>> ListarLecturasAsync(int idGuia, int idOperador)
        {
            var guia = await _context.Guias
                .AsNoTracking()
                .FirstOrDefaultAsync(
                      g =>
                      g.IdGuia == idGuia && (g.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada ||
                                            g.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida) &&
                                            g.IdUsuarioModificacion == idOperador);

            return await _context.LecturasGuia
                .AsNoTracking()
                .Include(lectura => lectura.Producto)
                .Include(lectura => lectura.UbicacionOrigen)
                .Include(lectura => lectura.UbicacionDestino)
                .Where(lectura => lectura.IdGuia == guia.IdGuia)
                .OrderByDescending(lectura => lectura.FechaRegistroUtc)
                .ThenByDescending(lectura => lectura.IdLecturaGuia)
                .ToListAsync();
        }

        public Task<LecturaGuia?> ObtenerLecturaGuiaAsync(int idGuia, long idLecturaGuia, int idOperador)
        {
            return _context.LecturasGuia.FirstOrDefaultAsync(lectura =>
                                                             lectura.IdLecturaGuia == idLecturaGuia &&
                                                             lectura.IdGuia == idGuia &&
                                                             lectura.IdUsuarioOperador == idOperador);
        }

        public async Task<IReadOnlySet<int>> ObtenerUbicacionesActivasAsync(
            int idAlmacen, IReadOnlyCollection<int> idsUbicaciones, bool soloDespacho)
        {
            if(idsUbicaciones.Count == 0)
                return new HashSet<int>();

            var query = _context.Ubicaciones
                .AsNoTracking()
                .Where(ubicacion =>
                ubicacion.IdAlmacen == idAlmacen &&
                ubicacion.Activo &&
                idsUbicaciones.Contains(ubicacion.IdUbicacion));

            if (soloDespacho)
            {
                query = query.Where(ubicacion => _context.UbicacionesDespacho.Any(despacho =>
                        despacho.IdAlmacen == idAlmacen && despacho.IdUbicacion == ubicacion.IdUbicacion));
            }

            var idsValidos = await query
                .Select(ubicacion => ubicacion.IdUbicacion)
                .ToListAsync();

            return idsValidos.ToHashSet();

        }

        public async Task<bool> ExisteOperadorAsync(int idOperador)
        {
             return await _context.Set<Usuario>().AnyAsync(u => u.IdUsuario == idOperador && u.Activo && 
                                                           u.rol != null && u.rol.Nombre.Trim().ToUpper() == "OPERADOR");
        }

        public Task<string?> ObtenerCodigoProductoAsync(int idProducto)
        {
            return _context.Productos
                            .AsNoTracking()
                            .Where(producto => producto.IdProducto == idProducto)
                            .Select(producto => producto.Codigo)
                            .FirstOrDefaultAsync();
        }

        public async Task<long> ObtenerCantidadStockAsync(int idUbicacion, int idProducto)
        {
            return await _context.StockUbicacion
                                    .AsNoTracking()
                                    .Where(stock => stock.IdUbicacion == idUbicacion && stock.IdProducto == idProducto)
                                    .Select(stock => (long?)stock.CantidadActual)
                                    .FirstOrDefaultAsync() ?? 0L;
        }

        public async Task<long> ObtenerCantidadLeidaAsync(int idGuia, int idProducto,int idUbicacion)
        {
            return await _context.LecturasGuia
                                 .AsNoTracking()
                                 .Where(lectura =>
                                        lectura.IdGuia == idGuia &&
                                        lectura.IdProducto == idProducto &&
                                        lectura.IdUbicacionOrigen == idUbicacion)
                                 .SumAsync(lectura => (long?)lectura.Cantidad) ?? 0L;

        }

        public Task<bool> TieneUbicacionesDeDespacho(int idAlmacen)
        {
            return
            _context.Ubicaciones.AsNoTracking()
                                .AnyAsync(ubicacion =>
                                    ubicacion.IdAlmacen == idAlmacen && ubicacion.Activo &&
                                            _context.UbicacionesDespacho.Any(despacho =>
                                                                             despacho.IdAlmacen == idAlmacen &&
                                                                             despacho.IdUbicacion == ubicacion.IdUbicacion));
        }



        public async Task GuardarCambiosAsync()
        {
             await _context.SaveChangesAsync();
        }

        public void AgregarLectura(LecturaGuia lectura)
        {
            _context.LecturasGuia.Add(lectura);
        }

        public void EliminarLectura(LecturaGuia lectura)
        {
            _context.LecturasGuia.Remove(lectura);
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

      
    }
}
