using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;

namespace ProyectoAW.Modules.Guias.Repositories;

public class GuiaRepository : IGuiaRepository
{
    private readonly AppDbContext _context;

    public GuiaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Guia>> ObtenerTodosAsync()
    {
        return await ConsultaGuias()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Guia?> ObtenerPorIdAsync(int id)
    {
        return await ConsultaGuias()
            .FirstOrDefaultAsync(guia => guia.IdGuia == id);
    }

    public async Task CrearAsync(Guia guia)
    {
        await _context.Guias.AddAsync(guia);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Guia guia)
    {
        // ObtenerPorIdAsync devuelve la guía seguida por EF Core; basta con guardar sus cambios.
        await _context.SaveChangesAsync();
    }

    private IQueryable<Guia> ConsultaGuias()
    {
        return _context.Guias
            .Include(guia => guia.TipoOperacion)
            .Include(guia => guia.Estado)
            .Include(guia => guia.Proveedor)
            .Include(guia => guia.Detalles)
                .ThenInclude(detalle => detalle.Producto);
    }
}
