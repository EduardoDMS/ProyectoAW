using ProyectoAW.Modules.Guias.DTOs.Requests;
using ProyectoAW.Modules.Guias.DTOs.Responses;

namespace ProyectoAW.Modules.Guias.Services;

public interface IMotivoGuiaService
{
    Task<List<MotivoGuiaDto>> ListarAsync(int? idOperacion, bool soloActivos);
    Task<MotivoGuiaDto> ObtenerAsync(int id);
    Task<List<TipoComportamientoGuiaDto>> ListarComportamientosAsync();
    Task<MotivoGuiaDto> CrearAsync(CrearMotivoGuiaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarMotivoGuiaDto dto);
    Task<bool> CambiarEstadoAsync(int id, bool activo);
    Task<bool> EliminarAsync(int id);
}
