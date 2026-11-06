using ProyectoAW.Modules.Guias.Entities;

namespace ProyectoAW.Modules.Guias.Repositories;

public interface IGuiaRepository
{
    Task<List<Guia>> ObtenerTodosAsync();
    Task<Guia?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Guia guia);
    Task ActualizarAsync(Guia guia);
}
