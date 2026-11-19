using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Seguridad.Security_Session.Repositories
{
    public interface IAuthRepository
    {
        Task<Usuario?> ObtenerPorUsernameAsync(string username);
    }
}
