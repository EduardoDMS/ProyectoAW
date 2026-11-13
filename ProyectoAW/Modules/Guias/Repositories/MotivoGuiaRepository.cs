using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Guias.Entities;

namespace ProyectoAW.Modules.Guias.Repositories;

public class MotivoGuiaRepository : IMotivoGuiaRepository
{
    private readonly AppDbContext _context;

    public MotivoGuiaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MotivoGuia>> ListarAsync(int? idOperacion, bool soloActivos)
    {
        var query = _context.MotivosGuia
            .AsNoTracking()
            .Include(motivo => motivo.TipoOperacionGuia)
            .Include(motivo => motivo.TipoComportamiento)
            .AsQueryable();

        if (idOperacion.HasValue)
            query = query.Where(motivo => motivo.IdTipoOperacionGuia == idOperacion.Value);

        if (soloActivos)
            query = query.Where(motivo => motivo.Activo);

        return await query.OrderBy(motivo => motivo.NombreMotivo).ToListAsync();
    }

    public async Task<MotivoGuia?> ObtenerAsync(int id)
    {
        return await _context.MotivosGuia
            .AsNoTracking()
            .Include(motivo => motivo.TipoOperacionGuia)
            .Include(motivo => motivo.TipoComportamiento)
            .FirstOrDefaultAsync(motivo => motivo.IdMotivoGuia == id);
    }

    public async Task<IReadOnlyList<TipoComportamientoGuia>> ListarComportamientosAsync()
    {
        return await _context.TiposComportamientoGuia
            .AsNoTracking()
            .OrderBy(tipo => tipo.Nombre)
            .ToListAsync();
    }

    public async Task<bool> ExisteOperacionAsync(int id)
    {
        return await _context.TiposOperacionesGuia.AnyAsync(operacion => operacion.IdTipoOperacionGuia == id);
    }

    public async Task<bool> ExisteComportamientoAsync(int id)
    {
        return await _context.TiposComportamientoGuia.AnyAsync(tipo => tipo.IdTipoComportamiento == id);
    }

    public async Task<bool> NombreExisteAsync(int idOperacion, string nombre, int? excluirId = null)
    {
        var query = _context.MotivosGuia
            .AsNoTracking()
            .Where(motivo => motivo.IdTipoOperacionGuia == idOperacion &&
                             motivo.NombreMotivo == nombre.Trim());

        if (excluirId.HasValue)
            query = query.Where(motivo => motivo.IdMotivoGuia != excluirId.Value);

        return await query.AnyAsync();
    }

    public async Task<bool> EstaEnUsoAsync(int id)
    {
        return await _context.Guias
            .AnyAsync(guia => guia.IdMotivoGuia == id);
    }

    public async Task AgregarAsync(MotivoGuia motivo)
    {
        await _context.MotivosGuia.AddAsync(motivo);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(MotivoGuia motivo)
    {
        _context.MotivosGuia.Update(motivo);
        await _context.SaveChangesAsync();
    }

    public void Eliminar(MotivoGuia motivo)
    {
        _context.MotivosGuia.Remove(motivo);
    }

    public async Task GuardarAsync()
    {
        await _context.SaveChangesAsync();
    }
}
