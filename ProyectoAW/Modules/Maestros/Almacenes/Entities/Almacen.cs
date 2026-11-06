using ProyectoAW.Modules.Maestros.Ubicaciones.Entities;
//using ProyectoAW.Modules.Stocks.Entities;
//using ProjectAW.Modules.Maestros.MovimientosStock.Entities;


namespace ProyectoAW.Modules.Maestros.Almacenes.Entities
{
    public class Almacen
    {
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; } // cambiar todos los maestros con a variable descripcion pase a llamarse Nombre 
        public bool Activo { get; set; } = true;
        public DateTime FchRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? FchModificacion { get; set; }

        // conexion a la nueva tabla

        public ICollection<Ubicacion>? Ubicaciones { get; set; }
        public ICollection<UbicacionDespacho> UbicacionesDespacho { get; set; } = new List<UbicacionDespacho>();
        //public ICollection<MovimientoStock>? MovimientoStock { get; set; }
        //public ICollection<StockAlmacen>? StockAlmacen { get; set; }


    }
}
