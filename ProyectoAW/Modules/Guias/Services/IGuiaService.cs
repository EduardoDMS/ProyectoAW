using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.DTOs.Responses;

namespace ProyectoAW.Modules.Guias.Services;

public interface IGuiaService
{
    Task<List<GuiaDto>> ObtenerTodosAsync();
    Task<GuiaDto?> ObtenerPorIdAsync(int id);
    Task<GuiaDto> CrearAsync(CrearGuiaDto request);
    Task<bool> ActualizarAsync(int id, ActualizarGuiaDto request);
    Task<bool> CancelarAsync(int id);
}
