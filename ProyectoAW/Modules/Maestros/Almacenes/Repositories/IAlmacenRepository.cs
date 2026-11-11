using ProyectoAW.Modules.Maestros.Almacenes.Entities;

namespace ProyectoAW.Modules.Maestros.Almacenes.Repositories
{
    public interface IAlmacenRepository
    {
        Task<List<Almacen>> ObtenerAlmacenesAsync();
        Task<Almacen?> ObtenerAlmacenPorIdAsync(int id);
        Task<Almacen?> ObtenerAlmacenPorCodigoAsync(string codigo);
        Task CrearAlmacenAsync(Almacen almacen);
        Task ActualizarAlmacenAsync(Almacen almacen);
        Task ActualizarAlmacenYUbicacionesDespachoAsync( Almacen almacen, IReadOnlyCollection<int> idsUbicacionesDespacho);
        Task<List<int>> ListarUbicacionesDelAlmacenAsync(int idAlmacen, IReadOnlyCollection<int> idsUbicaciones);
        Task EliminarUbicacionesDespachoAsync(int idAlmacen);
        Task ActualizarAsync(Almacen almacen);
    }
}
