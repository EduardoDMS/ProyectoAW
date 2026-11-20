using ProyectoAW.Modules.Guias.Entities;

namespace ProyectoAW.Modules.Guias.Repositories;

public interface IGuiaRepository
{
    Task<List<Guia>> ObtenerTodosAsync();
    Task<Guia?> ObtenerPorIdAsync(int id);
    Task<string?> ValidarReferenciasAsync(Guia guia);
    Task CrearAsync(Guia guia);
    Task ActualizarAsync(Guia guia);
    Task<List<Guia>> ListarGuiasTipoOperacionAsync(int idTipoOperacion);
}
