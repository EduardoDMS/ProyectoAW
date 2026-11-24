using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> ObtenerPorUsernameAsync(string username);
        Task<bool> TieneRegistrosAsociadosAsync(int idUsuario);
        Task CrearAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(Usuario usuario);
    }
}
