using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Services
{
    public interface IJwtService
    {
        string GeneracionToken(SesionResponse sesion);
    }
}
