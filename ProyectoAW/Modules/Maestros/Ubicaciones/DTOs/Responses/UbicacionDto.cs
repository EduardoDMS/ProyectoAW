namespace ProyectoAW.Modules.Maestros.Ubicaciones.DTOs.Responses
{
    public class UbicacionDto
    {
        public int IdUbicacion { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; } 
        public bool Activo { get; set; }
        public bool EsUbicacionDespacho { get; set; }
        public int IdAlmacen { get; set; }
        public string NombreAlmacen { get; set; } = string.Empty;
    }
}
