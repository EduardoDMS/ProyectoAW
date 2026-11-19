using Microsoft.AspNetCore.Identity;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Request;
using ProyectoAW.Modules.Seguridad.Security_Session.DTOs.Responses;
using ProyectoAW.Modules.Seguridad.Security_Session.Repositories;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;


namespace ProyectoAW.Modules.Seguridad.Security_Session.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IPasswordHasher<Usuario> _pwd;

        public AuthService(IAuthRepository repo, IPasswordHasher<Usuario> hasher)
        {
            _repo = repo;
            _pwd = hasher;
        }

        public async Task<SesionResponse?> ValidarCredencialAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var usuario = await _repo.ObtenerPorUsernameAsync(request.Username.Trim());

            if (usuario is null || !usuario.Activo || string.IsNullOrWhiteSpace(usuario.Password))
                return null;

            var resultado = _pwd.VerifyHashedPassword(usuario, usuario.Password, request.Password);
                
            if (resultado == PasswordVerificationResult.Failed)
                return null;

            return new SesionResponse(
                usuario.IdUsuario, usuario.Username, $"{usuario.Nombre} {usuario.Apellido}", usuario.IdRol,
                usuario.rol?.Nombre ?? string.Empty);
        }
    }
}
