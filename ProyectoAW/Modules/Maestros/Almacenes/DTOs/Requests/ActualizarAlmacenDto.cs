    namespace ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests
{
    public class ActualizarAlmacenDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool Activo { get; set; }
        public List<int> IdUbicacionesDespacho { get; set; } = [];
        public string FchModificacion { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
    }
}
