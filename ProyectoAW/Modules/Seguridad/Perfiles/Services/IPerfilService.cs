using ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Request;
using ProyectoAW.Modules.Seguridad.Perfiles.Dtos.Response;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Services;

public interface IPerfilService
{
    Task<List<string>> ListarModulosAsync();
    Task<List<PerfilDto>> ListarAsync();
    Task<PerfilDto> ObtenerPorIdAsync(int id);
    Task<PerfilDto> CrearAsync(CrearPerfilDto request);
    Task<bool> ActualizarAsync(int id, ActualizarPerfilDto request);
    Task<bool> EliminarAsync(int id);
}
