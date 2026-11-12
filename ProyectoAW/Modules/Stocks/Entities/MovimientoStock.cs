using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.Productos.Entities;
using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Stocks.Entities
{
    public class MovimientoStock
    {
        public int IdMovimientoStock { get; set; }
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public int IdTipoOperacion { get; set; }
        public int Cantidad { get; set; }
        public DateTime FchMovimiento { get; set; } = DateTime.Now;

        public int IdGuia { get; set; }
        public int IdUsuarioOpe { get; set; }
        public string Observacion { get; set; } = string.Empty;
        public int CantidadDiferenciada { get; set; }
        public int? IdUbicacionOrigen { get; set; }
        public int? IdUbicacionDestino { get; set; }

        public Producto? Producto { get; set; }
        public Almacen? Almacen { get; set; }
        public TipoOperacionGuia? OperacionGuias { get; set; }
        public Guia? Guia { get; set; }
        public Usuario? Usuario { get; set; } // tecnicamente solo reconocera a los usuarios que sean de rol operador :v pero aun estamos probando todo y faltaria crear ese proyecto asi que por ahora simulamos con un api 
        public Ubicacion? UbicacionOrigen { get; set; }
        public Ubicacion? UbicacionDestino { get; set; }
    }

}
