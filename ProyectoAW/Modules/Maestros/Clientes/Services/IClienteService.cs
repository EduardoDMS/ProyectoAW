using ProyectoAW.Modules.Maestros.Clientes.DTOs.Requests;
using ProyectoAW.Modules.Maestros.Clientes.DTOs.Responses;

namespace ProyectoAW.Modules.Maestros.Clientes.Services
{
    public interface IClienteService
    {
        Task<List<ClienteDto>> ObtenerTodosAsync();
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
        Task<ClienteDto?> ObtenerPorNumeroDocumento(string numeroDocumento);
        Task<ClienteDto> CrearAsync(CrearClienteDto dto);
        Task<bool> ActualizarAsync(int id, ActualizarClienteDto dto);
        Task<bool> CambiarEstadoAsync(int id, bool activo);
    }
}
