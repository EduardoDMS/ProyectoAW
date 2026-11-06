namespace ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests
{
    public class CrearAlmacenDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public List<int> IdUbicacionesDespacho { get; set; } = [];
        public DateTime FchRegistro { get; set; } = DateTime.Now;
    }
}
