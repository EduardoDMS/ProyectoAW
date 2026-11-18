using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Request;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Services
{
    public interface IAuthService
    {
        Task<SesionResponse?> ValidarCredencialAsync(LoginRequest request);
    }
}
