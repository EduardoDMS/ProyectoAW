using ProyectoAW.Modules.Seguridad.Roles.DTOs.Requests;
using ProyectoAW.Modules.Seguridad.Roles.DTOs.Response;

namespace ProyectoAW.Modules.Seguridad.Roles.Services
{
    public interface IRolService
    {
        Task<List<RolDto>> ObtenerTodosRolesAsync();
        Task<RolDto?> ObtenerRolPorIdAsync(int id);
        Task<RolDto?> ObtenerRolPorNombreAsync(string nombre);
        Task<RolDto> CrearRolAsync(CrearRolDto dto);
        Task<bool> ActualizarRolAsync(int id, ActualizarRolDto dto);
    }
}
