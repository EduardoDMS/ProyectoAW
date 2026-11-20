using ProyectoAW.Modules.Maestros.TipoComportamientos.DTOs.Responses;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Repositories;

namespace ProyectoAW.Modules.Maestros.TipoComportamientos.Services
{
    public class TipoComportamientoService : ITipoComportamientoService
    {
        private readonly ITipoComportamientosRepository _repo;

        public TipoComportamientoService(ITipoComportamientosRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<List<TipoComportamientoGuiaDto>> ListarTodo()
        {
            var listarTodos = await _repo.ListarComportamientos();

            return listarTodos.Select(t => new TipoComportamientoGuiaDto
            {
                IdTipoComportamiento = t.IdTipoComportamiento,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion
            }).ToList();
        }
    }
}
