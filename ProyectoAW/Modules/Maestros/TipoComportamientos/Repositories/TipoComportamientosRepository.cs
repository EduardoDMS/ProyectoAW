using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Entities;

namespace ProyectoAW.Modules.Maestros.TipoComportamientos.Repositories
{
    public class TipoComportamientosRepository : ITipoComportamientosRepository
    {
        private readonly AppDbContext _context;

        public TipoComportamientosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoComportamientoGuia>> ListarComportamientos()
        {
           return await _context.TiposComportamientoGuia.AsNoTracking().ToListAsync();
        }
    }
}
