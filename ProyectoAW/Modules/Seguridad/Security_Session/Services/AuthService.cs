using Microsoft.AspNetCore.Identity;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Request;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses;
using ProyectoAW.Modules.Seguridad.Security_Session.Repositories;
using ProyectoAW.Modules.Seguridad.Perfiles.Services;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;


namespace ProyectoAW.Modules.Seguridad.Security_Session.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;

        public AuthService(IAuthRepository repo)
        {
            _repo = repo;
           
        }

        public async Task<SesionResponse?> ValidarCredencialAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var usuario = await _repo.ObtenerPorUsernameAsync(request.Username.Trim());

            if (usuario is null ||
                !usuario.Activo ||
                string.IsNullOrWhiteSpace(usuario.Password) ||
                !string.Equals(usuario.Password, request.Password, StringComparison.Ordinal))
                return null;

            var rol = usuario.rol?.Nombre ?? string.Empty;
            var permisos = CatalogoPermisos.ObtenerPermisos(
                rol,
                usuario.PerfilUsuario?.Permisos ?? new List<ProyectoAW.Modules.Seguridad.Perfiles.Entites.PermisoPerfil>());

            return new SesionResponse(
                usuario.IdUsuario, usuario.Username, $"{usuario.Nombre} {usuario.Apellido}", usuario.IdRol,
                rol, permisos);
        }
    }
}
