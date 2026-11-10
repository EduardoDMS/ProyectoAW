namespace ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Requests
{
    public class ActualizarUnidadMedidaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
