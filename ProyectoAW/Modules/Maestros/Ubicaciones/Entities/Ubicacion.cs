using ProyectoAW.Modules.Maestros.Almacenes.Entities;

namespace ProyectoAW.Modules.Maestros.Ubicaciones.Entities
{
    public class Ubicacion
    {
        public int IdUbicacion { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FchRegistro { get; set; } = DateTime.Now;
        public DateTime? FchModificacion { get; set; }

        public int IdAlmacen { get; set; }
        public Almacen? Almacen { get; set; }
        public ICollection<UbicacionDespacho> UbicacionesDespacho { get; set; } = new List<UbicacionDespacho>();
    }
}


