using ProyectoAW.Modules.Seguridad.Usuarios.DTOs.Requests;
using ProyectoAW.Modules.Seguridad.Usuarios.DTOs.Responses;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ListarUsuariosAsync();
        Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int id);
        Task<UsuarioDto?> CrearUsuarioAsync(CrearUsuarioDto dto);
        Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto);
        Task<bool> CambiarEstadoAsync(int id, bool activo);
    }
}
