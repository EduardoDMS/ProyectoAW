using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
using ProyectoAW.Modules.Stocks.Entities;
//using ProyectoAW.Modules.Stocks.Entities;
//using ProjectAW.Modules.Maestros.MovimientosStock.Entities;


namespace ProyectoAW.Modules.Maestros.Almacenes.Entities
{
    public class Almacen
    {
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; } 
        public bool Activo { get; set; } = true;
        public string FchRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        public string? FchModificacion { get; set; }

        // conexion a la nueva tabla

        public ICollection<Ubicacion>? Ubicaciones { get; set; }
        public ICollection<UbicacionDespacho> UbicacionesDespacho { get; set; } = new List<UbicacionDespacho>();
        public ICollection<MovimientoStock>? MovimientoStock { get; set; }
        public ICollection<StockAlmacen>? StockAlmacen { get; set; }


    }
}
