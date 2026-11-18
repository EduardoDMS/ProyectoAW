namespace ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses
{
    public record SesionResponse
        (
            int IdUsuario,
            string Username,
            string NombreCompleto,
            int IdRol,
            string Rol
        );
    
}
