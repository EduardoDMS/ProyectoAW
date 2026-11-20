using ProyectoAW.Modules.Maestros.TipoComportamientos.DTOs.Responses;

namespace ProyectoAW.Modules.Maestros.TipoComportamientos.Services
{
    public interface ITipoComportamientoService
    {
        Task<List<TipoComportamientoGuiaDto>> ListarTodo();
    }
}
