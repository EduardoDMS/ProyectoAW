namespace ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests
{
    public class CrearAlmacenDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public List<int> IdUbicacionesDespacho { get; set; } = [];
        public string FchRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
    }
}
