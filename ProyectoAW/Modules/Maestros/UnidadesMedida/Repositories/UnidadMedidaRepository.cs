using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Entities;

namespace ProyectoAW.Modules.Maestros.UnidadesMedida.Repositories
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {

        private readonly AppDbContext _context;

        public UnidadMedidaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(UnidadMedida unidadMed)
        {
            _context.UnidadesMedida.Update(unidadMed);
            await _context.SaveChangesAsync();
        }

        public async Task CrearAsync(UnidadMedida unidadMed)
        {
            await _context.UnidadesMedida.AddAsync(unidadMed);
            await _context.SaveChangesAsync();
        }

        public async Task<UnidadMedida?> ObtenerPorCodigoAsync(string codigo)
        {
            return await _context.UnidadesMedida.AsTracking().FirstOrDefaultAsync(u => u.Codigo == codigo);
        }

        public async Task<UnidadMedida?> ObtenerPorIdAsync(int id)
        {
            return await _context.UnidadesMedida.AsNoTracking().FirstOrDefaultAsync(u => u.IdUnidadMedida == id);
        }

        public async Task<List<UnidadMedida>> ObtenerTodas()
        {
            return await _context.UnidadesMedida.AsNoTracking().ToListAsync();
        }
    }
}
