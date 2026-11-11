using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Almacenes.DTOs.Responses;

namespace ProyectoAW.Modules.Maestros.Almacenes.Services
{
    public interface IAlmacenService
    {
        Task<List<AlmacenDTO>> ObtenerAlmacenesTodosAsync();
        Task<AlmacenDTO?> ObtenerAlmacenPorIdAsync(int id);
        Task<AlmacenDTO?> ObtenerAlmacenPorCodigoAsync(string codigo);
        Task<AlmacenDTO> CrearAlmacenAsync(CrearAlmacenDto dto);

        Task<bool> CambiarEstadoAlmacenAsync(int id, bool activo);

        Task<bool> ActualizarAsync(int id, ActualizarAlmacenDto dto);
    }
}
