namespace ProyectoAW.Modules.Maestros.Ubicaciones.DTOs.Requests
{
    public class ActualizarUbicacionDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool Activo { get; set; }
        public string FchModificacion { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        public int IdAlmacen { get; set; }
    }
}
