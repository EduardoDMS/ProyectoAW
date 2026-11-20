using ProyectoAW.Modules.Maestros.MotivosGuia.DTOs.Requests;
using ProyectoAW.Modules.Maestros.MotivosGuia.DTOs.Responses;
using ProyectoAW.Modules.Maestros.TipoComportamientos.DTOs.Responses;

namespace ProyectoAW.Modules.Maestros.MotivosGuia.Services;

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
