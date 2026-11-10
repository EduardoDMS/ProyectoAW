using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;

namespace ProyectoAW.Modules.Maestros.UnidadesMedida.Repositories
{
    public interface IUnidadMedidaRepository
    {
        Task<List<UnidadMedida>> ObtenerTodas();
        Task<UnidadMedida?> ObtenerPorIdAsync(int id);
        Task<UnidadMedida?> ObtenerPorCodigoAsync(string codigo);
        Task CrearAsync(UnidadMedida unidadMed);
        Task ActualizarAsync(UnidadMedida unidadMed);
    }
}
