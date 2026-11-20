using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infrastructure.Data;
using ProyectoAW.Modules.Seguridad.Perfiles.Entites;

namespace ProyectoAW.Modules.Seguridad.Perfiles.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly AppDbContext _context;

    public PerfilRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Perfil>> ListarAsync()
    {
        return await _context.Perfiles
                             .AsNoTracking()
                             .Include(perfil => perfil.Permisos)
                             .OrderBy(perfil => perfil.NombrePerfil)
                             .ToListAsync();
    }

    public Task<Perfil?> ObtenerPorIdAsync(int id)
    {
        return _context.Perfiles
                       .Include(perfil => perfil.Permisos)
                       .FirstOrDefaultAsync(perfil => perfil.IdPerfil == id);
    }

    public async Task<bool> NombreExisteAsync(string nombre, int? excluirId = null)
    {
        var nombreNormalizado = nombre.Trim();
        var query = _context.Perfiles
                            .AsNoTracking()
                            .Where(perfil => perfil.NombrePerfil == nombreNormalizado);

        if (excluirId.HasValue)
            query = query.Where(perfil => perfil.IdPerfil != excluirId.Value);

        return await query.AnyAsync();
    }

    public Task<bool> TieneUsuariosAsignadosAsync(int idPerfil)
    {
        return _context.Usuarios
                       .AnyAsync(usuario => usuario.IdPerfilUsuario == idPerfil);
    }

    public async Task CrearAsync(Perfil perfil)
    {
        await _context.Perfiles.AddAsync(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Perfil perfil)
    {
        _context.Perfiles.Update(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Perfil perfil)
    {
        _context.Perfiles.Remove(perfil);
        await _context.SaveChangesAsync();
    }
}
