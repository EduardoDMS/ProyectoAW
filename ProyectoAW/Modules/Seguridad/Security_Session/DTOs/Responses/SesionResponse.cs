namespace ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses
{
    public sealed class SesionResponse
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string Rol { get; set; } = string.Empty;
        public List<string> Permisos { get; set; } = new();
    }
}
