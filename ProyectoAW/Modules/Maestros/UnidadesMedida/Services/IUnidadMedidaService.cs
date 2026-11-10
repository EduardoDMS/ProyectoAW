using ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Requests;
using ProyectoAW.Modules.Maestros.UnidadesMedida.DTOs.Responses;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;


namespace ProyectoAW.Modules.Maestros.UnidadesMedida.Services
{
    public interface IUnidadMedidaService
    {
        Task<List<UnidadMedidaDTO>> ObtenerTodasAsync();
        Task<UnidadMedidaDTO?> ObtenerPorIdAsync(int id);
        Task<UnidadMedidaDTO?> ObtenerPorCodigoAsync(string codigo);
        Task<UnidadMedidaDTO?> CrearAsync(CrearUnidadMedidaDto dto);
        Task<bool> ActualizarAsync(int id, ActualizarUnidadMedidaDto dto);
        Task<bool> CambiarEstadoAsync(int id, bool Activo);



    }
}
