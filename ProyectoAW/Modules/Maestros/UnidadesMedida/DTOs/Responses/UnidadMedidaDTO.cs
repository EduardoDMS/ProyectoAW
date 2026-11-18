namespace ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Responses
{
    public class UnidadMedidaDTO
    {
        public int IdUnidadMedida { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public bool Activo { get; set; }  // xdddd
    }
}
