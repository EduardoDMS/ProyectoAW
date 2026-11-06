using ProyectoAW.Modules.Seguridad.Roles.Entities;

namespace ProyectoAW.Modules.Seguridad.Roles.Repositories
{
    public interface IRolRepository
    {
        Task<List<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(int id);
        Task<Rol?> ObtenerPorNombreAsync(string username);
        Task CrearAsync(Rol rol);
        Task ActualizarAsync(Rol rol);
    }
}
