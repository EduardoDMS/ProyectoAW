using ProyectoAW.Modules.Maestros.TipoComportamientos.Entities;

namespace ProyectoAW.Modules.Maestros.TipoComportamientos.Repositories
{
    public interface ITipoComportamientosRepository
    {
        Task<List<TipoComportamientoGuia>> ListarComportamientos();
    }
}
