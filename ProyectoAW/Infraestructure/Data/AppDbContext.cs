using Microsoft.EntityFrameworkCore;
using ProyectoAW.Modules.Maestros.Estados.Entities;
using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Categorias.Entities;
using ProyectoAW.Modules.Maestros.Clientes.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.Productos.Entities;
using ProyectoAW.Modules.Maestros.Proveedores.Entities;
using ProyectoAW.Modules.Maestros.TipoDocumentos.Entities;
using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Seguridad.Roles.Entities;
//using ProyectoAW.Modules.Guias.Entities;
//using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Almacen> Almacenes => Set<Almacen>();
        public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();
        public DbSet<UbicacionDespacho> UbicacionesDespacho => Set<UbicacionDespacho>();
        public DbSet<UnidadMedida> UnidadesMedida => Set<UnidadMedida>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Guia> Guias => Set<Guia>();
        public DbSet<DetalleGuia> DetallesGuia => Set<DetalleGuia>();
        //public DbSet<CabeceraGuia> CabeceraGuia => Set<CabeceraGuia>();
        //public DbSet<MotivoGuia> MotivosGuia => Set<MotivoGuia>();
        //public DbSet<TipoComportamientoGuia> TiposComportamientoGuia => Set<TipoComportamientoGuia>();
        //public DbSet<DetalleGuia> DetalleGuia => Set<DetalleGuia>();
        public DbSet<TipoOperacionGuia> TiposOperacionesGuia => Set<TipoOperacionGuia>();
        public DbSet<EstadoGuia> EstadosGuia => Set<EstadoGuia>();
        public DbSet<TipoDocumento> TipoDocument => Set<TipoDocumento>();
        //public DbSet<StockAlmacen> StockAlmacen => Set<StockAlmacen>();
        //public DbSet<MovimientoStock> MovimientoStock => Set<MovimientoStock>();
        //public DbSet<StockUbicacion> StockUbicacion => Set<StockUbicacion>();
        //public DbSet<LecturaGuia> LecturasGuia => Set<LecturaGuia>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


            //// AppDbContext.OnModelCreating
            //modelBuilder.HasSequence<int>("SeqGuiaEntrada");
            //modelBuilder.HasSequence<int>("SeqGuiaSalida");
            //modelBuilder.HasSequence<int>("SeqGuiaTraslado");

        }
    }
}
