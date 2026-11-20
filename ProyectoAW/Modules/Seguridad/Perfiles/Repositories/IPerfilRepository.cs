using ProyectoAW.Modules.Seguridad.Perfiles.Entites;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Repositories;

public interface IPerfilRepository
{
    Task<List<Perfil>> ListarAsync();
    Task<Perfil?> ObtenerPorIdAsync(int id);
    Task<bool> NombreExisteAsync(string nombre, int? excluirId = null);
    Task<bool> TieneUsuariosAsignadosAsync(int idPerfil);
    Task CrearAsync(Perfil perfil);
    Task ActualizarAsync(Perfil perfil);
    Task EliminarAsync(Perfil perfil);
}
