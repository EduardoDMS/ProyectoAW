namespace ProyectoAW.Modules.Maestros.Ubicaciones.DTOs.Requests
{
    public class CrearUbicacionDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool Activo { get; set; }
        public string FchRegistro { get; set; } = ProyectoAW.Infrastructure.Data.DateTimeText.From(DateTime.Now);
        public int IdAlmacen { get; set; }

    }
}
